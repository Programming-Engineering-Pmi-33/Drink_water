namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FliudInfo
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        public FliudInfo()
        {
        }

        public List<Fluid> GetFluids()
        {
            return (from fluid in db.Fluids
                    select fluid).ToList();
        }
    }
}
