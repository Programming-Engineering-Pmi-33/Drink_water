using System;
using System.Collections.Generic;

#nullable disable

namespace DrinkWater
{
    public partial class Statistic
    {
        public int StatisticId { get; set; }

        public int UserIdRef { get; set; }

        public long FluidIdRef { get; set; }

        public long FluidAmount { get; set; }

        public DateTime Date { get; set; }

        public virtual Fluid FluidIdRefNavigation { get; set; }

        public virtual User UserIdRefNavigation { get; set; }
    }
}
