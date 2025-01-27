namespace Application.Business.Indicator
{
    public class AdxScores
    {
        public double Adx { get; private set; }
        public double Adxr { get; private set; }
        public double DIMinus { get; private set; }
        public double DIPlus{ get; private set; }

        public AdxScores(double adx, double adxr, double diMinus, double diPlus)
        {
            Adx = adx;
            Adxr = adxr;
            DIMinus = diMinus;
            DIPlus = diPlus;
        }
    }
    public class PivotPoints
    {
        public double Pivot { get; private set; }
        public double Support1 { get; private set; }
        public double Resistance1 { get; private set; }
        public double Support2 { get; private set; }
        public double Resistance2 { get; private set; }
        public DateTime Date { get; private set; }
        public PivotPoints(DateTime date, double high, double low, double close)
        {
            Date = date;
            Pivot = (high + low + close) / 3;
            Support1 = (2 * Pivot) - high;
            Resistance1 = (2 * Pivot) - low;
            Support2 = Pivot - (high - low);
            Resistance2 = Pivot + (high - low);
            
        }
        public PivotPoints(DateTime date, double pivot, double support1, double resistance1, double support2, double resistance2)
        {
            Date = date;
            Pivot = pivot;
            Support1 = support1;
            Resistance1 = resistance1;
            Support2 = support2;
            Resistance2 = resistance2;
        }
        public bool IsCalculated() {
            return Resistance2 != Pivot && Support2 != Pivot; // else the input data is not valid for calculating the pivot points
        }
    }
}
