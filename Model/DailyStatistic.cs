using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class DailyStatistic
    {
        [CustomNumberValid]
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        [CustomNumberValid]
        public long DailyBalance { get; set; }
        public bool WasAchieved { get; set; }

        public virtual User User { get; set; }
    }
}
