﻿using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class Users
    {
        public Users()
        {
            Statistics = new HashSet<Statistics>();
        }

        public long UserId { get; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public long? Weight { get; set; }
        public long? Height { get; set; }
        public string? Gender { get; set; }
        public long? Age { get; set; }
        public TimeSpan? WakeUp { get; set; }
        public TimeSpan? GoingToBed { get; set; }
        public DateTime? NotitficationsPeriod { get; set; }
        public bool? DisableNotifications { get; set; }
        public long? DailyBalance { get; set; }

        public virtual ICollection<Statistics> Statistics { get; set; }

        public Users(string username, string email,string password)
        {
            
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
