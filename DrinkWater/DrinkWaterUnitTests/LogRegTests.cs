using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrinkWater.Services;

namespace DrinkWaterUnitTests
{
    [TestClass]
    public class LogRegTests
    {
        private UsersService us=UsersService.GetService;
        

        [TestMethod]
        public void ExceptionIfUsernameIsNotInDatabase()
        {
            //arrange
            string username = "Keylu";

            //act
          bool isInDatabase=  us.UsernameExists(username);

            //assert
            Assert.IsFalse(isInDatabase);
        }

        [TestMethod]
        public void ExceptionIfEmailIsNotInDatabase()
        {
            //arrange
            string email = "gren@gmail.com";

            //act
            bool isInDatabase = us.EmailExists(email);

            //assert
            Assert.IsFalse(isInDatabase);
        }

        [TestMethod]
        public void SaltReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = "Katyau";

            //act
            string salt = us.GetUserSalt(username);

            //assert
            Assert.IsNotNull(salt);
        }

        [TestMethod]
        public void IdReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = "rekler";
            string password = "~???5GI?(:?5???????=";
            string salt = "304255195";

            //act
            int id = us.GetUserId(username,password,salt);

            //assert
            Assert.IsNotNull(id);
        }

        [TestMethod]
        public void RandomSaltIsCreatedIfMethodIsCorrect()
        {
            //arrange
            int salt;

            //act
            salt = EncryptionService.CreateRandomSalt();

            //assert
            Assert.IsNotNull(salt);
        }

        [TestMethod]
        public void SaltedHashIsCreatedIfMethodIsCorrect()
        {
            //arrange
            string saltedHash;

            //act
            saltedHash = EncryptionService.ComputeSaltedHash("abcdek", 304255195);

            //assert
            Assert.IsNotNull(saltedHash);
        }
    }
}
