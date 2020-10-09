using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class Statistic
    {
        [CustomNumberValid]
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public string LiqiudType { get; set; }
        [CustomNumberValid]
        public long Amount { get; set; }
        public int Id { get; set; }

        public virtual User User { get; set; }
    }
}
