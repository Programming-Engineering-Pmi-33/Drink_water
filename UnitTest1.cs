using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using  DrinkWater;

namespace DrinkWaterTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddNotification_UniqueId_True()
        {
            //arrange
            TestFunctions test = new TestFunctions(8);
            TimeSpan time = new TimeSpan(0, 0, 30);
            bool isNotificationInDatabase;

            //act
            test.AddNotifications(time, false);
            var list = test.db.Notifications.ToList();

            Notifications notification = (from res in list
                where res.UserId == 2
                select res).FirstOrDefault();


            if (notification == null)
            {
                isNotificationInDatabase = false;
            }
            else
            {
                isNotificationInDatabase = true;
            }

            //assert
            Assert.IsTrue(isNotificationInDatabase);
        }


        [TestMethod]
        public void AddNotification_NotUniqueId_False()
        {
            //arrange
            TestFunctions test = new TestFunctions(8);
            TimeSpan time = new TimeSpan(0, 1, 30);
            bool isNotificationInDatabase;

            //act
            test.AddNotifications(time, false);
            var list = test.db.Notifications.ToList();

            Notifications notification = (from res in list
                where res.UserId == 2
                select res).FirstOrDefault();


            if (notification == null)
            { 
                isNotificationInDatabase = false;
            }
            else
            {
                isNotificationInDatabase = true;
            }

            //assert
            Assert.IsTrue(isNotificationInDatabase);
        }


        [TestMethod]
        public void AddUser_CorrectData_True1()
        {
            //arrange
            TestFunctions test = new TestFunctions();

            bool isUserInDatabase;

            //act
            test.AddUser("Yura", "administrator", "grass@gmail.com", "grass", "grass");
            var list = test.db.User.ToList();

            User user = (from res in list
                where res.Username == "Yura"
                select res).FirstOrDefault();


            if (user == null)
            { 
                isUserInDatabase = false;
            }
            else
            {
                isUserInDatabase = true;
            }

            //assert
            Assert.IsTrue(isUserInDatabase, "no user in database.");
        }



        [TestMethod]
        public void AddUser_CorrectData_True2()
        {
            //arrange
            TestFunctions test = new TestFunctions();

            bool isUserInDatabase;

            //act
            test.AddUser("Oleh", "customer", "rock@gmail.com", "rock", "rock");
            var list = test.db.User.ToList();

            User user = (from res in list
                where res.Username == "Oleh"
                select res).FirstOrDefault();


            if (user == null)
            {
                isUserInDatabase = false;
            }
            else
            {
                isUserInDatabase = true;
            }

            //assert
            Assert.IsTrue(isUserInDatabase, "no user in database.");
        }

    }
}
