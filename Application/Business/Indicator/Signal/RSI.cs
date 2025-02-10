namespace Application.Business.Indicator.Signal
{
    public class RSI : ISignal
    {
        public double Value { get; private set; }
        public double Forecast { get; set; } = 0.0;
        public string Instrument { get; set; }
        public string Name { get; set; }
        public RSI(double value, string instrument)
        {
            Name = "RSI";
            Instrument = instrument;
            Value = value;
            if (value > 70)
                Forecast = -1.0;
            if (value < 30)
                Forecast = 1.0;
        }
    }
}
