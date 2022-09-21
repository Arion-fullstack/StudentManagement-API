using System.Net.Mail;

namespace StudentApiFact
{
    public class UnitTest
    {
        private readonly AuthController _authController;
        private readonly StudentController _studentController;
        private readonly StudentManagementContext _dbContext;

        public UnitTest()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            _dbContext = new StudentManagementContext();
            _authController = new AuthController(new AuthService(configuration, _dbContext));
            _studentController = new StudentController(new StudentService());
        }
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

        [Fact]
        public async Task TestRegisterOK()
        {
            UserRequest student = new UserRequest()
            {
                UserName = "testUsear123",
                Password = "123"
            };
            var result = await _authController.Register(student) as ObjectResult;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

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
        [Fact]
        public async Task TestGetListStudent()
        {
            int page = 1;
            var result = await _studentController.GetStudents(page) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetStudent()
        {
            int studentId = 45;
            var result = await _studentController.GetStudent(studentId) as ObjectResult;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task TestGetStudentNotFound()
        {
            int studentId = 1000;
            var result = await _studentController.GetStudent(studentId) as ObjectResult;
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }
    }
}