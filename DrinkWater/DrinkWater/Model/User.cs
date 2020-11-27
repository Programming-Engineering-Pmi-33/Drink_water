using System;
using System.Collections.Generic;

#nullable disable

namespace DrinkWater
{
    public partial class User
    {
        public User()
        {
            Statistics = new HashSet<Statistic>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long? Weight { get; set; }
        public long? Height { get; set; }
        public string Sex { get; set; }
        public long? Age { get; set; }
        public TimeSpan? WakeUp { get; set; }
        public TimeSpan? GoingToBed { get; set; }
        public TimeSpan? NotitficationsPeriod { get; set; }
        public bool? DisableNotifications { get; set; }
        public long? DailyBalance { get; set; }
        public byte[] Avatar { get; set; }
        public long? Salt { get; set; }

        public virtual ICollection<Statistic> Statistics { get; set; }
        public User(string username, string email, string password, long? salt)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.Salt = salt;
        }
    }
}
