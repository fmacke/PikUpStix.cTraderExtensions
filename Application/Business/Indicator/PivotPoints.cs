namespace Application.Business.Indicator
{
    public class PivotPoints
    {
        public double Pivot { get; private set; }
        public double Support1 { get; private set; }
        public double Resistance1 { get; private set; }
        public double Support2 { get; private set; }
        public double Resistance2 { get; private set; }
        public PivotPoints(double high, double low, double close)
        {
            Pivot = (high + low + close) / 3;
            Support1 = 2 * Pivot - high;
            Resistance1 = 2 * Pivot - low;
            Support2 = Pivot - (high - low);
            Resistance2 = Pivot + (high - low);
        }
    }
}
