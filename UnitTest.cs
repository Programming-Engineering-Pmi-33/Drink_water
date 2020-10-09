using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrinkWater;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class TestFunctionsUnitTests
    {
        [TestMethod]
        public void AddTimeTestEqual()
        {
            TestFunctions test = new TestFunctions(4);
            TimeSpan wakeup = new TimeSpan(9, 0, 0);
            TimeSpan goingbed = new TimeSpan(23, 0, 0);
            ActivityTime time = new ActivityTime { UserId = test.UserId, WakeUp = wakeup, GoingToBed = goingbed };
            test.AddActivitytime(wakeup, goingbed);
            ActivityTime last = test.db.ActivityTime.ToList().Last();
            Assert.IsTrue(last.Equals(time));

        }
        [TestMethod]
        public void AddTimeTestNotEqual()
        {
            TestFunctions test = new TestFunctions(6);
            TimeSpan wakeup = new TimeSpan(9, 0, 0);
            TimeSpan goingbed = new TimeSpan(23, 0, 0);
            TimeSpan goingbed2 = new TimeSpan(22, 0, 0);
            ActivityTime time = new ActivityTime { UserId = test.UserId, WakeUp = wakeup, GoingToBed = goingbed2 };
            test.AddActivitytime(wakeup, goingbed);
            ActivityTime last = test.db.ActivityTime.ToList().Last();
            Assert.IsFalse(last.Equals(time));

        }
        [TestMethod]
        public void AddUserParamsTestEqual()
        {
            TestFunctions test = new TestFunctions(7);
            long height = 196;
            long weight = 90;
            string gender = "female";
            UserParameters params1 = new UserParameters { UserId = test.UserId, Height = height, Weight = weight, Gender = gender };
            test.AddUserParameters(height,weight,gender);
            UserParameters last = test.db.UserParameters.ToList().Last();
            Assert.IsTrue(last.Equals(params1));

        }
        [TestMethod]
        public void AddUserParamsTestNotEqual()
        {
            TestFunctions test = new TestFunctions(1);
            long height = 196;
            long weight = 90;
            string gender = "female";
            long height2 = 180;
            UserParameters params1 = new UserParameters { UserId = test.UserId, Height = height, Weight = weight, Gender = gender };
            test.AddUserParameters(height2, weight, gender);
            UserParameters last = test.db.UserParameters.ToList().Last();
            Assert.IsFalse(last.Equals(params1));

        }
    }
}
