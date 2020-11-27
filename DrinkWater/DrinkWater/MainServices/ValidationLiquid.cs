namespace DrinkWater.MainServices
{
    using System.Linq;

    public class ValidationLiquid
    {
        private static string LiquidName { get; set; }

        private static long LiquidAmount { get; set; }

        public ValidationLiquid(string name, string amount)
        {
            if (IsValidAmount(amount))
            {
                LiquidName = new string(name.Where(char.IsLetter).ToArray());
                LiquidAmount = long.Parse(amount);
            }
        }

        public long GetAmount()
        {
            return LiquidAmount;
        }

        public string GetName()
        {
            return LiquidName;
        }

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
