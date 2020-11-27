using DrinkWater;
using DrinkWater.LogReg;
using DrinkWater.SettingServices;
using System.Linq;
using System.Windows.Media.Imaging;
using Xunit;
using DrinkWater.ProfileStatisticsServices;

namespace ProfileStatisticsUnitTests
{
    public class ScoreTests
    {

        
        [Theory]
        [InlineData(0, 7, 1, "4518_622_1720", 1, 3000)]
        [InlineData(0, 30, 1, "2356_4586_758_968_4254", 2, 4000)]
        [InlineData(0, 12, 12, "3625_2456_7845", 0, 1500)]

        public void ScoreTestPositiveTestMethod(int keepingBalanceDays, int totalDayNumber, int koef, string waterAmountPerPeriod, int result, long dailyBalance)
        {
            //Arrange
            var waterAmount = waterAmountPerPeriod.Split('_').Where(x => double.TryParse(x, out _)).Select(double.Parse).ToList();
            ScoreInfo scoreInfo = new ScoreInfo();

            //Act
            int actualResult = scoreInfo.Score(keepingBalanceDays, totalDayNumber, koef, waterAmount, dailyBalance);

            //Assert
            Assert.Equal(actualResult, result);
        }


        [Theory]
        [InlineData(0, 7, 1, "4518_622_1720", 1, 600)]
        [InlineData(0, 30, 1, "2356_4586_758_968_4254", 2, 900)]
        [InlineData(0, 12, 12, "3625_2456_7845", 3, 1500)]
        public void ScoreTestNegativeTestMethod(int keepingBalanceDays, int totalDayNumber, int koef, string waterAmountPerPeriod, int result, long dailyBalance)
        {
            //Arrange
            var waterAmount = waterAmountPerPeriod.Split('_').Where(x => double.TryParse(x, out _)).Select(double.Parse).ToList();
            ScoreInfo scoreInfo = new ScoreInfo();

            //Act
            int actualResult = scoreInfo.Score(keepingBalanceDays, totalDayNumber, koef, waterAmount, dailyBalance);

            //Assert
            Assert.NotEqual(actualResult, result);
        }
    }
    public class UserInfoTests : IDisposable
    {
        User user = new User();
        public UserInfoTests()
        {
            user = new User();
        }

        public void Dispose()
        {

        }

        [Theory]
        [InlineData(1, "Mamonchik", 106, 106, "Male", 20)]
        [InlineData(7, "Misha", 107, 195, "Male", 20)]
        [InlineData(10, "Mish", 110, 195, "Male", 20)]
        public void GetUserInfoPositiveTestMethod(int id, string expectedUsername, long expectedWeight, long expectedHeight, string expectedSex, long expectedAge)
        {
            //Arrange
            SessionUser sessionUser = new SessionUser(id, expectedUsername);
            UserData userData = new UserData(sessionUser);

            //Act
            User userInformation = userData.GetData();

            //Assert
            Assert.Equal(expectedWeight, userInformation.Weight);
            Assert.Equal(expectedHeight, userInformation.Height);
            Assert.Equal(expectedSex, userInformation.Sex);
            Assert.Equal(expectedAge, userInformation.Age);
        }

        [Theory]
        [InlineData(1, "Mamonchik", 14, 50, "Female", 10)]
        [InlineData(7, "Misha", 17, 95, "Female", 10)]
        [InlineData(10, "Mish", 10, 95, "Female", 10)]
        public void GetUserInfoNegativeTestMethod(int id, string expectedUsername, long expectedWeight, long expectedHeight, string expectedSex, long expectedAge)
        {
            //Arrange
            SessionUser sessionUser = new SessionUser(id, expectedUsername);
            UserData userData = new UserData(sessionUser);

            //Act
            User userInformation = userData.GetData();

            //Assert
            Assert.NotEqual(expectedWeight, userInformation.Weight);
            Assert.NotEqual(expectedHeight, userInformation.Height);
            Assert.NotEqual(expectedSex, userInformation.Sex);
            Assert.NotEqual(expectedAge, userInformation.Age);
        }

        [Fact]
        public void ShowAvatarPositiveTestMethod()
        {
            //Arrange
            SessionUser sessionUser = new SessionUser(1, "Mamonchik");
            UserData userData = new UserData(sessionUser);
            User userInformation = userData.GetData();

            //Act
            BitmapImage image = new ImageHandler().GetImagefromDB(userInformation.Avatar);

            //Assert
            Assert.False(image == null);
        }
        [Fact]
        public void ShowAvatarNegativeTestMethod()
        {
            //Arrange
            SessionUser sessionUser = new SessionUser(17, "Misha");
            UserData userData = new UserData(sessionUser);
            User userInformation = userData.GetData();

            //Act
            BitmapImage image = new ImageHandler().GetImagefromDB(userInformation.Avatar);

            //Assert
            Assert.True(image == null);
        }

        [Theory]
        [InlineData(22, 8, 14)]
        [InlineData(23, 8, 15)]
        [InlineData(24, 9, 15)]
        [InlineData(1, 9, 16)]
        public void GetUserActivityTimePositiveTestMethod(int bedtime, int wakeuptime, int result)
        {
            //Arrange
            UserInfo uInfo = new UserInfo();

            //Act
            int actual = uInfo.GetUserActivityTime(bedtime, wakeuptime);

            //Assert
            Assert.Equal(result, actual);
        }

        [Theory]
        [InlineData(22, 8, 85)]
        [InlineData(23, 8, 35)]
        [InlineData(24, 9, 55)]
        public void GetUserActivityTimeNegativeTestMethod(int bedtime, int wakeuptime, int result)
        {
            //Arrange
            UserInfo uInfo = new UserInfo();

            //Act
            int actual = uInfo.GetUserActivityTime(bedtime, wakeuptime);

            //Assert
            Assert.NotEqual(result, actual);
        }

    }
   
}
