using DrinkWater.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrinkWaterUnitTests
{

    [TestClass]
    public class LogRegTests
    {
        private UsersService us=UsersService.GetService;

        [TestMethod]
        public void NotExceptionIfUsernameIsInDatabase()
        {
            //arrange
            string username = "Mishaq";

            //act
          bool isInDatabase=us.UsernameExists(username);

            //assert
            Assert.IsTrue(isInDatabase);
        }

        [TestMethod]
        public void NotExceptionIfEmailIsInDatabase()
        {
            //arrange
            string email = "mishqa@gmail.com";

            //act
            bool isInDatabase = us.EmailExists(email);

            //assert
            Assert.IsTrue(isInDatabase);
        }

        [TestMethod]
        public void SaltReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = "KatyaU";


            //act
            string salt = us.GetUserSalt(username);

            //assert
            Assert.IsNotNull(salt);
        }

        [TestMethod]
        public void IdReceivedIfUsernameIsInDatabase()
        {
            //arrange
            string username = "KatyaU";
            string password = "vqcCyM9XGZvrxdSInyNoE0yRj1I=";
            string salt = "1918332768";

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
