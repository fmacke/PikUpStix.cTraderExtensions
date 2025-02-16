namespace Application.Business.Indicator
{
    public class Ewmac
    {
        public decimal GetEwmac(decimal currentPeriodDAta, decimal decay, decimal previousPeriodEWMA)
        {
            decimal x = decimal.Round(currentPeriodDAta * decay + previousPeriodEWMA * (1 - decay), 6);
            return x;
        }
    }
}