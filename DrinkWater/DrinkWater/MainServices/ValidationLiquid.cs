namespace DrinkWater.MainServices
{
    using System.Linq;

    /// <summary>Class <c>ValidationLiquid</c>the model checks the correctness of the fluid data .
    /// </summary>
    public class ValidationLiquid
    {
        private static string LiquidName { get; set; }

        private static long LiquidAmount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationLiquid"/> class.
        /// </summary>
        /// <param name="name">Liquid name.</param>
        /// <param name="amount">Liquid amount.</param>
        public ValidationLiquid(string name, string amount)
        {
            if (IsValidAmount(amount))
            {
                LiquidName = new string(name.Where(char.IsLetter).ToArray());
                LiquidAmount = long.Parse(amount);
            }
        }

        /// <summary>
        /// Gets liquid amount.
        /// </summary>
        /// <returns>Amount of liquid.</returns>
        public long GetAmount()
        {
            return LiquidAmount;
        }

        /// <summary>
        /// Gets name of liquid.
        /// </summary>
        /// <returns>Liquid name.</returns>
        public string GetName()
        {
            return LiquidName;
        }

        /// <summary>This method determines whether text data about
        /// the amount of liquid can be parsed to long type.</summary>
        /// <returns>True, if the text is correct for parsing
        /// and in the range from 0 to 6000 milliliters, otherwise false.</returns>
        /// <param name="text">is the data from textbox about the amount of liquid.</param>
        public bool IsValidAmount(string text)
        {
            bool isValid = false;
            long test;
            if (long.TryParse(text, out test) && test > 0 && test < 6000)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
