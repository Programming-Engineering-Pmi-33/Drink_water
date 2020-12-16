namespace ScoreTests
{
    using System.Linq;
    using DrinkWater.ProfileStatisticsServices;
    using Xunit;

    /// <summary>
    /// Check score functionality.
    /// </summary>
    public class ScoreTests
    {
        /// <summary>
        /// Positive score functionality test.
        /// </summary>
        /// <param name="keepingBalanceDays">Number of days keeping daily balance.</param>
        /// <param name="totalDayNumber">Total number of days in period.</param>
        /// <param name="koef">Constant for identification of period.</param>
        /// <param name="waterAmountPerPeriod">Array of water consumed per day for every period.</param>
        /// <param name="result">Normal amount of water for user.</param>
        /// <param name="dailyBalance">Number of days keeping daily balance per period.</param>
        [Theory]
        [InlineData(0, 7, 1, "4518_622_1720", 1, 3000)]
        [InlineData(0, 30, 1, "2356_4586_758_968_4254", 2, 4000)]
        [InlineData(0, 12, 12, "3625_2456_7845", 0, 1500)]
        public void ScoreTestPositiveTestMethod(int keepingBalanceDays, int totalDayNumber, int koef, string waterAmountPerPeriod, int result, long dailyBalance)
        {
            // Arrange
            var waterAmount = waterAmountPerPeriod.Split('_').Where(x => double.TryParse(x, out _)).Select(double.Parse).ToList();
            ScoreInfo scoreInfo = new ScoreInfo();

            // Act
            int actualResult = scoreInfo.Score(keepingBalanceDays, totalDayNumber, koef, waterAmount, dailyBalance);

            // Assert
            Assert.Equal(actualResult, result);
        }

        /// <summary>
        /// Negative score functionality test.
        /// </summary>
        /// <param name="keepingBalanceDays">Number of days keeping daily balance.</param>
        /// <param name="totalDayNumber">Total number of days in period.</param>
        /// <param name="koef">Constant for identification of period.</param>
        /// <param name="waterAmountPerPeriod">Array of water consumed per day for every period.</param>
        /// <param name="result">Normal amount of water for user.</param>
        /// <param name="dailyBalance">Number of days keeping daily balance per period.</param>
        [Theory]
        [InlineData(0, 7, 1, "4518_622_1720", 1, 600)]
        [InlineData(0, 30, 1, "2356_4586_758_968_4254", 2, 900)]
        [InlineData(0, 12, 12, "3625_2456_7845", 3, 1500)]
        public void ScoreTestNegativeTestMethod(int keepingBalanceDays, int totalDayNumber, int koef, string waterAmountPerPeriod, int result, long dailyBalance)
        {
            // Arrange
            var waterAmount = waterAmountPerPeriod.Split('_').Where(x => double.TryParse(x, out _)).Select(double.Parse).ToList();
            ScoreInfo scoreInfo = new ScoreInfo();

            // Act
            int actualResult = scoreInfo.Score(keepingBalanceDays, totalDayNumber, koef, waterAmount, dailyBalance);

            // Assert
            Assert.NotEqual(actualResult, result);
        }
    }
}
