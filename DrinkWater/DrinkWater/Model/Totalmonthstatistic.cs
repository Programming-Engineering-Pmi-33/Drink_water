#nullable disable

namespace DrinkWater
{
    using System;
    using System.Collections.Generic;

<<<<<<<< HEAD:DrinkWater/DrinkWater/Model/Totalmonthstatistic.cs
    public partial class Totalmonthstatistic
========
    public partial class Totalweekstatistics
>>>>>>>> Misha_branch:DrinkWater/DrinkWater/Model/Weekstatistic.cs
    {
        public int? UserIdRef { get; set; }

        public long? FluidIdRef { get; set; }

        public decimal? Sum { get; set; }
    }
}
