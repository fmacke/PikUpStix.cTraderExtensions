namespace Application.Business.Indicator.Signal
{
    public class MovingAverages : ISignal
    {
        public double Value { get; private set; }
        public double Forecast { get; set; } = 0.0;
        public string Instrument { get; set; }
        public string Name { get; set; }
        public MovingAverages(double shortMA, double mediumMA, double longMA, string instrument)
        {
            Name = "MovingAverages";
            Instrument = "Instrument";

            double signal = 0.0;

            if (shortMA > mediumMA && mediumMA > longMA)
            {
                signal = 1.0; // Strong Buy Signal
            }
            else if (shortMA < mediumMA && mediumMA < longMA)
            {
                signal = -1.0; // Strong Sell Signal
            }
            else
            {
                // Calculate intermediate signals based on the difference between moving averages
                double diff1 = shortMA - mediumMA;
                double diff2 = mediumMA - longMA;
                signal = (diff1 + diff2) / 2;
            }

            // Normalize signal to be between -1 and 1
            if (signal > 1.0) signal = 1.0;
            if (signal < -1.0) signal = -1.0;

            Value = signal;
            Forecast = signal;
        }
    }
}
