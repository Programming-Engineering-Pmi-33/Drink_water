using System;
using System.Linq;

namespace DrinkWater
{
    public class TestFunctions
    {
        public long UserId { get; set; }
        private bool Exist { get; set; } = false;
        public dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();
        public TestFunctions() { }
        public TestFunctions(int userId)
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
        public void AddActivitytime(TimeSpan wakeUp, TimeSpan goingToBed)
        {
            ActivityTime newActivityTime = new ActivityTime { UserId = UserId, WakeUp = wakeUp, GoingToBed = goingToBed };
            db.ActivityTime.Add(newActivityTime);
            db.SaveChanges();
        }
        public void AddUserParameters(long height, long weight, string gender)
        {
            UserParameters newUserParameters = new UserParameters { Height = height, Weight = weight, Gender = gender, UserId = UserId };
            db.UserParameters.Add(newUserParameters);
            db.SaveChanges();
        }
    }
}
