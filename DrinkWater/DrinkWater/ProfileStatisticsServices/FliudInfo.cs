namespace DrinkWater.ProfileStatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class fot getting fluids from database.
    /// </summary>
    public class FliudInfo
    {
        private static dfkg9ojh16b4rdContext db = new dfkg9ojh16b4rdContext();

        /// <summary>
        /// Initializes a new instance of the <see cref="FliudInfo"/> class.
        /// </summary>
        public FliudInfo()
        {
        }

        /// <summary>
        /// Get fluids list.
        /// </summary>
        /// <returns>Flids list.</returns>
        public List<Fluid> GetFluids()
        {
            return (from fluid in db.Fluids
                    select fluid).ToList();
        }
    }
}
