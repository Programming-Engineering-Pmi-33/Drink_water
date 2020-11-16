using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DrinkWater.LogReg;

namespace DrinkWater.SettingServices
{
    class UserData
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        private Users User { get; set; }

        public UserData()
        {
        }

        public UserData(SessionUser sessionUser)
        {
            User = (from searchingUser in db.Users
                        where searchingUser.UserId == sessionUser.UserId
                        select searchingUser).FirstOrDefault();
        }

        public Users GetData()
        {
            return User;
        }

        public void SetUserParameters(long weight, long height, long age, string sex, TimeSpan wakeUp, TimeSpan goingToBed)
        {
            User.Weight = weight;
            User.Height = height;
            User.Age = age;
            User.Sex = sex;
            User.WakeUp = wakeUp;
            User.GoingToBed = goingToBed;
            db.SaveChanges();
        }

        public void SetUserInformation(string username, string password, string email, byte[] imageArray)
        {
            User.Username = username;
            User.Password = password;
            User.Email = email;
            if (imageArray != null)
            {
                User.Avatar = imageArray;
            }

            db.SaveChanges();
        }

        public void SetUserNotitfications(string choosenText, int customPeriod, bool isDisabled)
        {
            if (choosenText.Contains("Custom"))
            {
                User.NotitficationsPeriod = new TimeSpan(customPeriod, 0, 0);
            }

            switch (choosenText)
            {
                case "Every hour":
                    User.NotitficationsPeriod = new TimeSpan(1, 0, 0);
                    break;

                case "Every day":
                    User.NotitficationsPeriod = new TimeSpan(24, 0, 0);
                    break;

                default:
                    break;
            }

            User.DisableNotifications = Convert.ToBoolean(isDisabled);
            db.SaveChanges();
        }
    }
}
