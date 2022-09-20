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
         //   _authController = new AuthController(new AuthService(configuration, _dbContext));
            _studentController = new StudentController(new StudentService());
        }

    }
}