using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class UserParameters
    {
        [CustomNumberValid]
        public long UserId { get; set; }
        [CustomNumberValid]
        public long Height { get; set; }
        [CustomNumberValid]
        public long Weight { get; set; }
        [CustomNumberValid]
        public string Gender { get; set; }

        public virtual User User { get; set; }
    }
}
