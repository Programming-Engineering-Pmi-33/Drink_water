namespace DrinkWaterProfileStatisticsUnitTests
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Media.Imaging;
    using DrinkWater;
    using DrinkWater.LogReg;
    using DrinkWater.ProfileStatisticsServices;
    using DrinkWater.Services;
    using DrinkWater.SettingServices;
    using Xunit;

    /// <summary>
    /// Check user information functionality.
    /// </summary>
    public class UserInfoTests : IDisposable
    {
        private UsersService usersService = UsersService.GetService;

        /// <summary>
        /// First test user.
        /// </summary>
        private User user1 = new User();

        /// <summary>
        /// Second test user.
        /// </summary>
        private User user2 = new User();

        /// <summary>
        /// Third test user.
        /// </summary>
        private User user3 = new User();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoTests"/> class.
        /// Test set-up.
        /// </summary>
        public UserInfoTests()
        {
            using (Bitmap bitmap = new Bitmap(@"C:\Users\Anastasiia\Pictures\272498.png"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    var imageArray = ms.ToArray();
                    this.user1 = new User("mamonchik@gmail.com", "qwerty123456", 1000000, "Mamonchik", 106, 106, "Male", 20, imageArray);
                }
            }
            this.usersService.RegisterUser(this.user1);
            this.user2 = new User("misha@gmail.com", "qwerty123456", 1000001, "Misha", 107, 195, "Male", 20, null);
            this.usersService.RegisterUser(this.user2);
            this.user3 = new User("mish@gmail.com", "qwerty123456", 1000002, "Mish", 110, 195, "Male", 20, null);
            this.usersService.RegisterUser(this.user3);
        }

        /// <summary>
        /// Test tear down.
        /// </summary>
        public void Dispose()
        {
            this.usersService.DeleteUser(this.user1);
            this.usersService.DeleteUser(this.user2);
            this.usersService.DeleteUser(this.user3);
        }

        /// <summary>
        /// Checks that user nformation was stored correctly.
        /// </summary>
        /// <param name="id">user id.</param>
        /// <param name="expectedUsername">expected username.</param>
        /// <param name="expectedWeight">expected user weight.</param>
        /// <param name="expectedHeight">expected user height.</param>
        /// <param name="expectedSex">expected user sex.</param>
        /// <param name="expectedAge">expected user age.</param>
        [Theory]
        [InlineData(1000000, "Mamonchik", 106, 106, "Male", 20)]
        [InlineData(1000001, "Misha", 107, 195, "Male", 20)]
        [InlineData(1000002, "Mish", 110, 195, "Male", 20)]
        public void GetUserInfoPositiveTestMethod(int id, string expectedUsername, long expectedWeight, long expectedHeight, string expectedSex, long expectedAge)
        {
            // Arrange
            SessionUser sessionUser = new SessionUser(id, expectedUsername);
            UserData userData = new UserData(sessionUser);

            // Act
            User userInformation = userData.GetData();

            // Assert
            Assert.Equal(expectedWeight, userInformation.Weight);
            Assert.Equal(expectedHeight, userInformation.Height);
            Assert.Equal(expectedSex, userInformation.Sex);
            Assert.Equal(expectedAge, userInformation.Age);
        }

        /// <summary>
        /// Positive avatar show test.
        /// </summary>
        [Fact]
        public void ShowAvatarPositiveTestMethod()
        {
            // Arrange
            SessionUser sessionUser = new SessionUser(this.user1.UserId, this.user1.Username);
            UserData userData = new UserData(sessionUser);
            User userInformation = userData.GetData();

            // Act
            BitmapImage image = new ImageHandler().GetImagefromDB(userInformation.Avatar);

            // Assert
            Assert.False(image == null);
        }

        /// <summary>
        /// Negative avatar show test.
        /// </summary>
        [Fact]
        public void ShowAvatarNegativeTestMethod()
        {
            // Arrange
            SessionUser sessionUser = new SessionUser(this.user2.UserId, this.user2.Username);
            UserData userData = new UserData(sessionUser);
            User userInformation = userData.GetData();

            // Act
            BitmapImage image = new ImageHandler().GetImagefromDB(userInformation.Avatar);

            // Assert
            Assert.True(image == null);
        }

        /// <summary>
        /// Positive activity time test.
        /// </summary>
        /// <param name="bedtime">Time that user is going to bed.</param>
        /// <param name="wakeuptime">Time that user wakes up.</param>
        /// <param name="result">Time that user is active through daytime.</param>
        [Theory]
        [InlineData(22, 8, 14)]
        [InlineData(23, 8, 15)]
        [InlineData(24, 9, 15)]
        [InlineData(1, 9, 16)]
        public void GetUserActivityTimePositiveTestMethod(int bedtime, int wakeuptime, int result)
        {
            // Arrange
            UserInfo uInfo = new UserInfo();

            // Act
            int actual = uInfo.GetUserActivityTime(bedtime, wakeuptime);

            // Assert
            Assert.Equal(result, actual);
        }

        /// <summary>
        /// Negative activity time test.
        /// </summary>
        /// <param name="bedtime">Time that user is going to bed.</param>
        /// <param name="wakeuptime">Time that user wakes up.</param>
        /// <param name="result">Time that user is active through daytime.</param>
        [Theory]
        [InlineData(22, 8, 85)]
        [InlineData(23, 8, 35)]
        [InlineData(24, 9, 55)]
        public void GetUserActivityTimeNegativeTestMethod(int bedtime, int wakeuptime, int result)
        {
            // Arrange
            UserInfo uInfo = new UserInfo();

            // Act
            int actual = uInfo.GetUserActivityTime(bedtime, wakeuptime);

            // Assert
            Assert.NotEqual(result, actual);
        }
    }
}
