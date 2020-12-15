using System;
using System.Collections.Generic;

#nullable disable

namespace DrinkWater
{
    public partial class Dailystatistic
    {
#pragma warning disable SA1600 // Elements should be documented
        public int? UserIdRef { get; set; }

        public long? FluidIdRef { get; set; }

        public double? Sum { get; set; }
#pragma warning restore SA1600 // Elements should be documented

    }
}
