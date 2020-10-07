using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class ActivityTime
    {
        public long UserId { get; set; }
        public TimeSpan WakeUp { get; set; }
        public TimeSpan GoingToBed { get; set; }

        public virtual User User { get; set; }
    }
}
