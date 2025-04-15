namespace Application.Business.Risk
{
    public class RiskManager : IRiskManager
    {
        public RiskManager(double forecast, double maximumRisk, double accountBalance, double pipSize, double stopLoss, double entryPrice)
        {
            Forecast = forecast;
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
            StopLoss = stopLoss;
            EntryPrice = entryPrice;
            CalculateLotSize();
        }
        public double Forecast { get; private set; } = 0.0;
        public double RiskPerTrade { get; private set; } = 0.0;
        public double AccountBalance { get; private set; } = 0.0;
        public double PipSize { get; private set; } = 0.0;
        public double StopLoss { get; private set; } = 0.0;
        public double EntryPrice { get; private set; } = 0.0;
        public double LotSize { get; private set; } = 0.0;
        private void CalculateLotSize()
        {
            var risk = AccountBalance * RiskPerTrade / (StopLoss * EntryPrice);
            LotSize = risk * (1/PipSize);  
        }
    }

}
