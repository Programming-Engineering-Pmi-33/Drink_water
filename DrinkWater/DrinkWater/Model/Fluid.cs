#nullable disable

namespace DrinkWater
{
    using System;
    using System.Collections.Generic;

    public partial class Fluid
    {
        public Fluid()
        {
            Statistics = new HashSet<Statistic>();
        }

        public long FluidId { get; set; }

        public string Name { get; set; }

        public double Koeficient { get; set; }

        public byte[] FliudImage { get; set; }

        public virtual ICollection<Statistic> Statistics { get; set; }
    }
}
