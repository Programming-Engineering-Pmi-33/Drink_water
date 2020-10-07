using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class User
    {
        public User()
        {
            Statistic = new HashSet<Statistic>();
        }

        public long UserId { get; set; }
        public string Role { get; set; }
        [CustomNameValid]
        public string Username { get; set; }
        [CustomPasswordValid]
        public string Password { get; set; }

        public virtual ActivityTime ActivityTime { get; set; }
        public virtual DailyStatistic DailyStatistic { get; set; }
        public virtual Notifications Notifications { get; set; }
        public virtual UserParameters UserParameters { get; set; }
        public virtual ICollection<Statistic> Statistic { get; set; }
    }
}
