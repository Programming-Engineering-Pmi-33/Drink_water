namespace DrinkWater
{
    using System.Collections.Generic;

    public partial class Fluids
    {
        public Fluids()
        {
            Statistics = new HashSet<Statistics>();
        }

        public long FluidId { get; set; }

        public string Name { get; set; }

        public double Koeficient { get; set; }

        public byte[] FliudImage { get; set; }

        public virtual ICollection<Statistics> Statistics { get; set; }
    }
}
