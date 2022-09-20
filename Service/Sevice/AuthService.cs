using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;
using Service.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service.Sevice
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly StudentManagementContext _studentManagementContext;
        public AuthService(IConfiguration configuration, StudentManagementContext studentManagementContext)
        {
            _configuration = configuration;
            _studentManagementContext = studentManagementContext;
        }
        public MyResponseObject<UserResponse> Login(UserRequest user)
        {
            var userDB = _studentManagementContext.Users.Where(t => t.Username.Equals(user.UserName)).FirstOrDefault();
            if (userDB == null)
            {
                return new MyResponseObject<UserResponse>
                {
                    Code = 404,
                    Message = "Username does not exist!",
                };
            }
            else
            {
                if (VerifyPasswordHash(user.Password, userDB.PasswordHash, userDB.PasswordSalt))
                {
                    var token = CreateToken(userDB.Id);
                    var refreshToken = GenerateRefreshToken(userDB.Id);

                    _studentManagementContext.RefreshTokens.Add(refreshToken);
                    _studentManagementContext.SaveChanges();
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = refreshToken.Expires
                    };
                    return new MyResponseObject<UserResponse>
                    {
                        Code = 200,
                        Data = new UserResponse
                        {
                            Token = token,
                            RefreshToken = refreshToken.Token,
                            Username = userDB.Username
                        },
                        Message = "Login Success!",
                    };
                }
                return new MyResponseObject<UserResponse>
                {
                    Code = 401,
                    Message = "Incorrect password!",
                };
            }
        }
        public async Task<MyResponseObject<UserResponse>> RefreshToken(string refreshToken)
        {
            if (refreshToken == null)
            {
                return new MyResponseObject<UserResponse>
                {
                    Code = 401,
                    Message = "Unauthorized",
                };
            }

            var refreshTokenDB =
                await _studentManagementContext.RefreshTokens
                        .Where(r => r.Token.Equals(refreshToken))
                        .FirstOrDefaultAsync();

            if (refreshTokenDB != null)
            {
                int id = refreshTokenDB.Id;

                //Remove Token old
                _studentManagementContext.RefreshTokens.Remove(refreshTokenDB);
                await _studentManagementContext.SaveChangesAsync();

                //Create Token
                string newToken = CreateToken(refreshTokenDB.UserId);

                //GenerateRefreshToken
                var generateRefreshToken = GenerateRefreshToken(refreshTokenDB.UserId);

                //Save token in db
                await SaveRefreshToken(generateRefreshToken);

                //Create Cookie Option
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = generateRefreshToken.Expires
                };

                return new MyResponseObject<UserResponse>
                {
                    Code = 200,
                    Data = new UserResponse
                    {
                        Token = newToken,
                        RefreshToken = generateRefreshToken.Token,
                    },
                    Message = "Ok",
                };
            }
            else
            {
                return new MyResponseObject<UserResponse>
                {
                    Code = 403,
                    Message = "Forbidden",
                };
            }
        }
        public async Task<MyResponseObject<object>> Register(UserRequest userRequest)
        {
            var userDB = _studentManagementContext.Users.Where(t => t.Username.Equals(userRequest.UserName)).FirstOrDefault();
            if (userDB != null)
                return new MyResponseObject<object>()
                {
                    Code = 409,
                    Message = "Username already exists!",
                };

            CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User()
            {
                Username = userRequest.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _studentManagementContext.Users.AddAsync(user);
            await _studentManagementContext.SaveChangesAsync();

            return new MyResponseObject<object>()
            {
                Code = 200,
                Message = "Sign Up Success!",
                Data = new
                {
                    Username = user.Username
                }
            };
        }

        public async Task<MyResponseObject<object>> Logout(string refreshToken)
        {

            if (refreshToken == null)
            {
                return new MyResponseObject<object>
                {
                    Code = 401,
                    Message = "Unauthorized",
                };
            }

            var refreshTokenDB =
                await _studentManagementContext.RefreshTokens
                        .Where(r => r.Token.Equals(refreshToken))
                        .FirstOrDefaultAsync();

            if (refreshTokenDB != null)
            {
                int id = refreshTokenDB.Id;

                //Remove Token old
                _studentManagementContext.RefreshTokens.Remove(refreshTokenDB);
                await _studentManagementContext.SaveChangesAsync();

                return new MyResponseObject<object>
                {
                    Code = 200,
                    Message = "Logout successful",
                };
            }
            else
            {
                return new MyResponseObject<object>
                {
                    Code = 403,
                    Message = "Forbidden",
                };
            }
        }
        #region Token
        public string CreateToken(int userId)
        {
            List<Claim> claims = claimsRoles(userId);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddSeconds(60),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public List<Claim> claimsRoles(int userId)
        {
            List<Claim> claims = new List<Claim>();

            var roles = _studentManagementContext.Roles.Where(t => t.UserId == userId).ToList();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Name));
                }
            }
            claims.Add(new Claim(ClaimTypes.Name, "_UHAIHSA002"));
            return claims;
        }

        public async Task SaveRefreshToken(RefreshToken refreshToken)
        {
            await _studentManagementContext.RefreshTokens.AddAsync(refreshToken);
            await _studentManagementContext.SaveChangesAsync();
        }
        public RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                UserId = userId
            };
            return refreshToken;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion
    }
}
