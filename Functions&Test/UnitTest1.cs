using Microsoft.VisualStudio.TestTools.UnitTesting;
using Drink_water;
using System;

namespace UnitTest_Statistics
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddDailyStatistic_True()
        {
            TestFunctionsStatistics DailyStatistics2 = new TestFunctionsStatistics(5);
            DailyStatistics2.AddDailyStatistic(DateTime.Today, 2000, true);
            DailyStatistics2.DoesExist();
            Assert.IsTrue(DailyStatistics2.Exist);
        }

        [TestMethod]
        public void AddDailyStatistic_False()
        {
            TestFunctionsStatistics DailyStatistics2 = new TestFunctionsStatistics(3);
            DailyStatistics2.AddDailyStatistic(DateTime.Parse("07-10-2020"), 2000, false);
            DailyStatistics2.DoesExist();
            Assert.IsTrue(DailyStatistics2.Exist);
        }
        [TestMethod]
        public void AddDailyStatistic_existingUser_False()
        {
            TestFunctionsStatistics DailyStatistics2 = new TestFunctionsStatistics(2);
            DailyStatistics2.AddDailyStatistic(DateTime.Today, 2000, false);
            DailyStatistics2.DoesExist();
            Assert.IsTrue(DailyStatistics2.Exist);
        }

        [TestMethod]
        public void AddStatistic1_True()
        {
            TestFunctionsStatistics satistics = new TestFunctionsStatistics(1);
            satistics.AddStatistic(DateTime.Today, "water",2000);
            satistics.AddStatistic(DateTime.Parse("06-10.2020"), "tea", 500);
            satistics.DoesExist();
            Assert.IsTrue(satistics.Exist);
        }
        [TestMethod]
        public void AddStatistic1_False()
        {
            TestFunctionsStatistics satistics = new TestFunctionsStatistics(22);
            satistics.AddStatistic(DateTime.Parse("07-10.2020"), "milk", 2000);
            satistics.AddStatistic(DateTime.Today, "bear", 500);
            satistics.DoesExist();
            Assert.IsTrue(satistics.Exist);
        }
    }
}
