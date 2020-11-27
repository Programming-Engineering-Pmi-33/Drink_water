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
        private static UsersService us=UsersService.GetService;

        [ClassInitialize]
        public static void TestsSetup(TestContext context)
        {
            user1 = new User("rekler", "rekler@gmail.com", "reklerRekleR", "3456789089");
            us.RegisterUser(user1);
            user2 = new User("lemig", "lemig@gmail.com", "lemigLemiG", "3456789024");
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
           string salt = us.GetUserSalt(username);

            //assert
            Assert.IsNotNull(salt);
        }

        [TestMethod]
        public void IdReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = user1.Username;
            string password = EncryptionService.ComputeSaltedHash(user1.Password,int.Parse(user1.Salt));
            string salt = user1.Salt;

            //act
            int id = us.GetUserId(username,password,salt);

            //assert
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void RandomSaltIsCreatedIfMethodIsCorrect()
        {
            //arrange
            int salt1;
            int salt2;

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

            //act
            saltedHash1 = EncryptionService.ComputeSaltedHash(user1.Password, int.Parse(user1.Salt));
            saltedHash2 = EncryptionService.ComputeSaltedHash(user2.Password, int.Parse(user2.Salt));

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
