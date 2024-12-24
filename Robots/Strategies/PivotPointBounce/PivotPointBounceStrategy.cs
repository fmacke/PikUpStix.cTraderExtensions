namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointStrategy 
    {
        public decimal Pivot { get; private set; }
        public decimal Support1 { get; private set; }
        public decimal Resistance1 { get; private set; }
        public decimal Support2 { get; private set; }
        public decimal Resistance2 { get; private set; }

        public PivotPointStrategy(decimal high, decimal low, decimal close)
        {
            CalculatePivotPoints(high, low, close);
        }

        private void CalculatePivotPoints(decimal high, decimal low, decimal close)
        {
            Pivot = (high + low + close) / 3;
            Support1 = 2 * Pivot - high;
            Resistance1 = 2 * Pivot - low;
            Support2 = Pivot - (high - low);
            Resistance2 = Pivot + (high - low);
        }

        public string DetermineEntryPoint(decimal currentPrice)
        {
            if (currentPrice <= Support1)
            {
                return "Long Entry at Support1";
            }
            else if (currentPrice >= Resistance1)
            {
                return "Short Entry at Resistance1";
            }
            return "No Entry";
        }

        public string DetermineExitPoint(decimal entryPrice, bool isLong)
        {
            if (isLong)
            {
                if (entryPrice <= Support1)
                {
                    return $"Take Profit at Pivot or Resistance1";
                }
            }
            else
            {
                if (entryPrice >= Resistance1)
                {
                    return $"Take Profit at Pivot or Support1";
                }
            }
            return "No Exit";
        }
    }
}
