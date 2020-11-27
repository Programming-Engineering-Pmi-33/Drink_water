using DrinkWater;
using DrinkWater.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrinkWaterUnitTests
{
    [TestClass]
    public class LogRegTests
    {
        private static User user1;
        private static User user2;
        private static UsersService us = UsersService.GetService;

        [ClassInitialize]
        public static void TestsSetup(TestContext context)
        {
            user1 = new User("rekler", "rekler@gmail.com", "reklerRekleR", 3456789089);
            us.RegisterUser(user1);
            user2 = new User("lemig", "lemig@gmail.com", "lemigLemiG", 3456789024);
            us.RegisterUser(user2);
        }

        [TestMethod]
        public void NotExceptionIfUsernameIsInDatabase()
        {
            //arrange
            string username = user1.Username;

            //act
          bool isInDatabase=us.UsernameExists(username);

            //assert
            Assert.IsTrue(isInDatabase);
        }

        [TestMethod]
        public void NotExceptionIfEmailIsInDatabase()
        {
            //arrange
            string email = user1.Email;

            //act
            bool isInDatabase = us.EmailExists(email);

            //assert
            Assert.IsTrue(isInDatabase);
        }

        [TestMethod]
        public void SaltIsRecievedIfUsernameIsInDatabase()
        {
            //arrange
            string username = user1.Username;


            //act
           long salt = us.GetUserSalt(username);

            //assert
            Assert.IsNotNull(salt);
        }

        [TestMethod]
        public void IdReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = user1.Username;
            long salt = us.GetUserSalt(user1.Username);
            string password = EncryptionService.ComputeSaltedHash(user1.Password, salt);

            //act
            int id = us.GetUserId(username,password,salt);

            //assert
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void RandomSaltIsCreatedIfMethodIsCorrect()
        {
            //arrange
            long salt1;
            long salt2;

            //act
            salt1 = EncryptionService.CreateRandomSalt();
            salt2 = EncryptionService.CreateRandomSalt();

            //assert
            Assert.AreNotEqual(salt1,salt2);
        }

        [TestMethod]
        public void SaltedHashIsCreatedIfMethodIsCorrect()
        {
            //arrange
            string saltedHash1;
            string saltedHash2;
            long salt1 = us.GetUserSalt(user1.Username);
            long salt2 = us.GetUserSalt(user2.Username);

            //act
            saltedHash1 = EncryptionService.ComputeSaltedHash(user1.Password, salt1);
            saltedHash2 = EncryptionService.ComputeSaltedHash(user2.Password,salt2);

            //assert
            Assert.AreNotEqual(saltedHash1,saltedHash2);
        }

        [ClassCleanup]
        public static void TestsTearDown()
        {
            us.DeleteUser(user1);
            us.DeleteUser(user2);
        }
    }
}
