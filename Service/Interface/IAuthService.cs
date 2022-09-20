using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAuthService
    {
        Task<MyResponseObject<object>> Register(UserRequest user);
        MyResponseObject<UserResponse> Login(UserRequest user);
        Task<MyResponseObject<object>> Logout(string refreshToken);
        Task<MyResponseObject<UserResponse>> RefreshToken(string refreshTokenStr);
        string CreateToken(int userId);
        RefreshToken GenerateRefreshToken(int userId);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
