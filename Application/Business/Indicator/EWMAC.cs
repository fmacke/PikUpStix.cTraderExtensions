namespace Application.Business.Indicator
{
    public class Ewmac
    {
        public double GetEwmac(double currentPeriodDAta, double decay, double previousPeriodEWMA)
        {
            double x = Math.Round(currentPeriodDAta * decay + previousPeriodEWMA * (1 - decay), 6);
            return x;
        }
    }
}