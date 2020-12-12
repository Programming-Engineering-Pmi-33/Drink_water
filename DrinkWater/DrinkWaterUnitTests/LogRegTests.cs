namespace DrinkWaterUnitTests
{
    using DrinkWater;
    using DrinkWater.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Announces LogRegTests ñlass.
    /// </summary>
    [TestClass]
    public class LogRegTests
    {
        private static User user1;
        private static User user2;
        private static UsersService userService = UsersService.GetService;

        /// <summary>
        /// Sets data for tests.
        /// </summary>
        /// <param name="context">test context instance.</param>
        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            user1 = new User("rekler", "rekler@gmail.com", "reklerRekleR", 3456789089);
            userService.RegisterUser(user1);
            user2 = new User("lemig", "lemig@gmail.com", "lemigLemiG", 3456789024);
            userService.RegisterUser(user2);
        }

        /// <summary>
        /// Deletes data from database.
        /// </summary>
        [ClassCleanup]
        public static void TearDown()
        {
            userService.DeleteUser(user1);
            userService.DeleteUser(user2);
        }

        /// <summary>
        /// Checks that no exception if username is in database.
        /// </summary>
        [TestMethod]
        public void NoExceptionIfUsernameIsInDatabase()
        {
            // arrange
            string username = user1.Username;

            // act
            bool isInDatabase = userService.UsernameExists(username);

            // assert
            Assert.IsTrue(isInDatabase);
        }

        /// <summary>
        /// Checks that no exception if email is in database.
        /// </summary>
        [TestMethod]
        public void NoExceptionIfEmailIsInDatabase()
        {
            // arrange
            string email = user1.Email;

            // act
            bool isInDatabase = userService.EmailExists(email);

            // assert
            Assert.IsTrue(isInDatabase);
        }

        /// <summary>
        /// Checks if salt is received if username is in database.
        /// </summary>
        [TestMethod]
        public void SaltIsReceivedIfUsernameIsInDatabase()
        {
            // arrange
            long salt = user1.Salt;

            // assert
            Assert.IsNotNull(salt);
        }

        /// <summary>
        /// Checks if id is received if username is in database.
        /// </summary>
        [TestMethod]
        public void IdReceivedIfUsernameIsInDatabase()
        {
            // arrange
            string username = user1.Username;
            long salt = user1.Salt;
            string password = EncryptionService.ComputeSaltedHash(user1.Password, salt);

            // act
            int id = userService.GetUserId(username, password, salt);

            // assert
            Assert.IsNotNull(id);
        }

        /// <summary>
        /// Checks if salt is created using correct method.
        /// </summary>
        [TestMethod]
        public void RandomSaltIsCreatedIfMethodIsCorrect()
        {
            // arrange
            long salt1;
            long salt2;

            // act
            salt1 = user1.Salt;
            salt2 = user2.Salt;

            // assert
            Assert.AreNotEqual(salt1, salt2);
        }

        /// <summary>
        /// Checks if salted hash is computed using correct method.
        /// </summary>
        [TestMethod]
        public void SaltedHashIsComputedIfMethodIsCorrect()
        {
            // arrange
            string saltedHash1;
            string saltedHash2;
            long salt1 = user1.Salt;
            long salt2 = user2.Salt;

            // act
            saltedHash1 = EncryptionService.ComputeSaltedHash(user1.Password, salt1);
            saltedHash2 = EncryptionService.ComputeSaltedHash(user2.Password, salt2);

            // assert
            Assert.AreNotEqual(saltedHash1, saltedHash2);
        }
    }
}
