using System;
using System.Collections.Generic;

namespace DrinkWater
{
    public partial class Monthstatistic
    {
        public int? UserIdRef { get; set; }
        public long? FluidIdRef { get; set; }
        public decimal? Sum { get; set; }
        public DateTime? Date { get; set; }
    }
}
