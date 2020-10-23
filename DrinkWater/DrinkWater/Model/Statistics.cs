using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class Statistics
    {
        public int StatisticId { get; set; }
        public int UserIdRef { get; set; }
        public long FluidIdRef { get; set; }
        public long FluidAmount { get; set; }
        public DateTime Date { get; set; }

        public virtual Fluids FluidIdRefNavigation { get; set; }
        public virtual Users UserIdRefNavigation { get; set; }
    }
}
