namespace Application.Business.Indicator
{
    public interface IIndicator
    {
    }
    public interface IIndicator<T> : IIndicator
    {
        T Calculate(List<double> data, int period);
    }
    public class EMA : IIndicator<List<double>>
    {
        public List<double> Calculate(List<double> data, int period)
        {
            if (data == null || data.Count < period || period <= 0)
                throw new ArgumentException("Invalid data or period for EMA calculation.");

            List<double> emaValues = new List<double>();
            double multiplier = 2.0 / (period + 1);

            // Calculate the first EMA value (typically the SMA of the first 'period' values)
            double currentEma = data.Take(period).Average();
            emaValues.Add(currentEma);

            // Calculate subsequent EMA values
            for (int i = period; i < data.Count; i++)
            {
                currentEma = ((data[i] - currentEma) * multiplier) + currentEma;
                emaValues.Add(currentEma);
            }
            return emaValues;
        }
    }
    public class RSI : IIndicator<List<double>>
    {
        public List<double> Calculate(List<double> prices, int period)
        {
            if (prices == null || prices.Count <= period)
            {
                return new List<double>();
            }

            List<double> rsiValues = new List<double>();
            List<double> gains = new List<double>();
            List<double> losses = new List<double>();

            for (int i = 1; i < prices.Count; i++)
            {
                double change = prices[i] - prices[i - 1];
                gains.Add(Math.Max(0, change));
                losses.Add(Math.Max(0, -change));
            }

            // Calculate initial average gain and loss
            double avgGain = gains.Take(period).Average();
            double avgLoss = losses.Take(period).Average();

            double rs = (avgLoss == 0) ? 100 : (avgGain / avgLoss); // Handle division by zero
            rsiValues.Add(100 - (100 / (1 + rs)));

            // Calculate subsequent RS and RSI values
            for (int i = period; i < gains.Count; i++)
            {
                avgGain = (avgGain * (period - 1) + gains[i]) / period;
                avgLoss = (avgLoss * (period - 1) + losses[i]) / period;

                rs = (avgLoss == 0) ? 100 : (avgGain / avgLoss); // Handle division by zero
                rsiValues.Add(100 - (100 / (1 + rs)));
            }
            return rsiValues;
        }
    }
}
