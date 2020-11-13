using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DrinkWater.ProfileStatisticsServices
{
    class FliudInfo
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        public FliudInfo() 
        {
        }

        public List<Fluids> GetFluids()
        {
            return (from fluid in db.Fluids
                    select fluid).ToList();
        }
    }
}
