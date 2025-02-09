namespace Application.Business.Risk
{
    public class RiskManager : IRiskManager
    {
        public RiskManager(double forecast, double maximumRisk, double accountBalance, double pipSize)
        {
            if (forecast > 1)
                Forecast = 1.0;
            if(forecast < -1)
                Forecast = -1.0;
            if(forecast < 0)
                Forecast = forecast * -1;
            if (forecast != 0)
                RiskPerTrade = maximumRisk * Forecast;
            AccountBalance = accountBalance;
            PipSize = pipSize;
        }
        public double Forecast { get; private set; } = 0.0;
        public double RiskPerTrade { get; private set; } = 0.0;
        public double AccountBalance { get; private set; } = 0.0;
        public double PipSize { get; private set; } = 0.0;
        public double CalculateLotSize(double stopLossPips, double entryPrice)
        {
            var lotSize = AccountBalance * RiskPerTrade / (stopLossPips * entryPrice);
            return lotSize * (1/PipSize);  
        }
    }

}
