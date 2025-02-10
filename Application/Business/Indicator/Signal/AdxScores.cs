namespace Application.Business.Indicator.Signal
{
    public class AdxScores : ISignal
    {
        public double Adx { get; private set; }
        public double Adxr { get; private set; }
        public double DIMinus { get; private set; }
        public double DIPlus { get; private set; }
        public double Forecast { get; set; } = 0.0;
        public string Instrument { get; set; }
        public string Name { get; set; }

        public AdxScores(double adx, double adxr, double diMinus, double diPlus, string instrument, double lowThreshold, double highthreshold)
        {
            Name = "ADX";
            Instrument = instrument;
            Adx = adx;
            Adxr = adxr;
            DIMinus = diMinus;
            DIPlus = diPlus;
            if (adx > lowThreshold)
            {
                if (adx > highthreshold)
                    Forecast = 1.0;
                else
                {
                    var range = highthreshold - lowThreshold;
                    var current = adx - lowThreshold;
                    var ratio = current / range;
                    if (diPlus > diMinus)
                        Forecast = ratio;
                    else
                        Forecast = -1.0 * ratio;
                }
            }
        }
    }
}
