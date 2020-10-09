using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class Notifications
    {
        public long UserId { get; set; }
        public TimeSpan Period { get; set; }
        public bool IsDisabled { get; set; }

        public virtual User User { get; set; }
    }
}
