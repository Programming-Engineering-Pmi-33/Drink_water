namespace DrinkWater.SettingServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DrinkWater.LogReg;
    using DrinkWater.Services;

    /// <summary>
    /// Calss for working with user information.
    /// </summary>
    public class UserData
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        private User User { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// Empty constructor for <c>User</c> class.
        /// </summary>
        public UserData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserData"/> class.
        /// </summary>
        /// <param name="sessionUser">object of <c>SessionUser</c> class.</param>
        public UserData(SessionUser sessionUser)
        {
            User = (from searchingUser in db.Users
                        where searchingUser.UserId == sessionUser.UserId
                        select searchingUser).FirstOrDefault();
        }

        /// <summary>
        /// Getter for <c>User</c> property.
        /// </summary>
        /// <returns>User property.</returns>
        public User GetData()
        {
            return User;
        }

        /// <summary>
        /// This function get user daily balance.
        /// </summary>
        /// <returns>User daily balance as long.</returns>
        public long? GetDailyBalnace()
        {
            var userDailyBalance = (from searchingUser in db.Users
                    where searchingUser.UserId == User.UserId
                    select searchingUser.DailyBalance).FirstOrDefault();
            if (userDailyBalance == null || userDailyBalance == 0)
            {
                return 0;
            }
            else
            {
                return userDailyBalance;
            }
        }

        /// <summary>
        /// This function edit user parameters settings in database.
        /// </summary>
        /// <param name="weight">User weight value.</param>
        /// <param name="height">User height value.</param>
        /// <param name="age">User age value.</param>
        /// <param name="sex">User sex value.</param>
        /// <param name="wakeUp">Value when user wakes up.</param>
        /// <param name="goingToBed">Value of time whe user goes to bed.</param>
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

        /// <summary>
        /// This function edit user parameters information in database.
        /// </summary>
        /// <param name="username">User name value.</param>
        /// <param name="password">User password value.</param>
        /// <param name="email">User email value.</param>
        /// <param name="imageArray">User avatar value.</param>
        public void SetUserInformation(string username, string password, string email, byte[] imageArray)
        {
            User.Username = username;
            if (User.Password != password)
            {
                long salt = EncryptionService.CreateRandomSalt();
                User.Password = EncryptionService.ComputeSaltedHash(password, salt);
                User.Salt = salt;
            }

            User.Email = email;
            if (imageArray != null)
            {
                User.Avatar = imageArray;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// This function edit user notifications settings in database.
        /// </summary>
        /// <param name="choosenText">Option choosen by user.</param>
        /// <param name="customPeriod">Period inputed by user.</param>
        /// <param name="isDisabled">Does user disable notifications.</param>
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
