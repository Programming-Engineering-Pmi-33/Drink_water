using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrinkWater.Model
{
    class TestFunctions
    {
        private long UserId { get; set; }
        private bool Exist { get; set; } = false;
        dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        TestFunctions() { }
        TestFunctions(int userId)
        {
            UserId = userId;
        }
        private void DoesExist()
        {
            var Users = db.User.ToList();
            User searched_user = (from User user in Users
                where user.UserId == UserId
                select user).FirstOrDefault();
            if (searched_user != null)
            {
                Exist = true;
            }
        }
        private void AddUser(string username, string role, string email, string password, string confirmedPassword)
        {
            if (password == confirmedPassword)
            {
                User newUser = new User { Username = username, Role = role, Password = password };
                db.User.Add(newUser);
                db.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Passwords does not match");
            }
        }
        public void AddNotifications(TimeSpan period, bool isDisabled)
        {
            Notifications newNotifications = new Notifications { UserId = UserId, Period = period, IsDisabled = isDisabled };
            db.Notifications.Add(newNotifications);
            db.SaveChanges();
        }
    }
}
