using Microsoft.EntityFrameworkCore;
using Service.Sevice;
using System.Net.Mail;

namespace StudentApiFact
{
    public class UnitTest
    {
        private readonly AuthController _authController;
        private readonly StudentController _studentController;
        private readonly StudentManagementContext _dbContext;
        private readonly AuthService _authService;

        public UnitTest()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            _dbContext = new StudentManagementContext();
            _authService = new AuthService(configuration, _dbContext);
            _authController = new AuthController(_authService);
            _studentController = new StudentController(new StudentService());
        }

        #region test login
        /// <summary>
        /// Login successful
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestLoginOk()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "admin",
                Password = "123"
            };
            var result = _authController.Login(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        /// <summary>
        /// Login with username does not exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestLoginNotFoundUsername()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "stringdqwdwq",
                Password = "string"
            };
            var result = _authController.Login(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }


        /// <summary>
        /// Login with incorrect password
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestLoginPasswordFail()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "admin",
                Password = "stringdqwqwd"
            };
            var result = _authController.Login(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        #endregion

        #region test register
        /// <summary>
        /// Register successful
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestRegisterOK()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "ewqewqe",
                Password = "123"
            };
            var result = await _authController.Register(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        /// <summary>
        /// Register with username is exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestRegisterAlreadyExistst()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "admin",
                Password = "123"
            };
            var result = await _authController.Register(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
        }

        #endregion

        #region test student
        /// <summary>
        /// Get list student
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestGetListStudent()
        {
            int page = 1;
            var result = await _studentController.GetStudents(page) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        /// <summary>
        /// Get student by id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestGetStudent()
        {
            int studentId = 45;
            var result = await _studentController.GetStudent(studentId) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        /// <summary>
        /// Get student by id does'n exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestGetStudentNotFound()
        {
            int studentId = 1000;
            var result = await _studentController.GetStudent(studentId) as ObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }


        /// <summary>
        /// Test update student by id ok
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestUpdateStudentOK()
        {
            Student student = new Student()
            {
                Id = 45,
                Email = "hungtran1530@gmail.com",
                FirstName = "Tran",
                LastName = "Hung"
            };

            var result = await _studentController.PutStudent(student.Id, student) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }


        [Fact]
        public async Task TestUpdateStudentNotFound()
        {
            Student student = new Student()
            {
                Id = 10212122,
                Email = "hungtran1530@gmail.com",
                FirstName = "Tran",
                LastName = "Hung"
            };

            var result = await _studentController.PutStudent(student.Id, student) as ObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        #endregion
    }

    public class UnitTestService{

        private readonly AuthService _authService;
        private readonly StudentManagementContext _dbContext;

        public UnitTestService()
        {


            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
            _dbContext = new StudentManagementContext();
            _authService = new AuthService(configuration, _dbContext);
        }


        #region test login
        /// <summary>
        /// Test AuthService Method Login
        /// Pass
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestLoginService()
        {
            UserRequest userRequest = new UserRequest()
            {
                UserName= "admin",
                Password = "123"
            };
            var result = _authService.Login(userRequest);

            Assert.NotNull(result.Data);
        }

        /// <summary>
        /// Test AuthService Method Login
        /// Fail: username not found
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestLoginServiceNotFound()
        {
            UserRequest userRequest = new UserRequest()
            {
                UserName = "ue",
                Password = "da"
            };
            var result = _authService.Login(userRequest);

            Assert.Null(result.Data);
        }
        #endregion
    }
}