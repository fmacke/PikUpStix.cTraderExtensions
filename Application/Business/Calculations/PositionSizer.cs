namespace Application.Business.Calculations
{
    public class PositionSizer : ICalculate
    {
        public double Forecast { get; private set; } = 0.0;
        public double RiskPerTrade { get; private set; } = 0.0;
        public double AccountBalance { get; private set; } = 0.0;
        public double PipSize { get; private set; } = 0.0;
        public double LotSize { get; private set; } = 0.0;
        public double StopLossPrice { get; private set; } = 0.0;
        public double EntryPrice { get; private set; } = 0.0;
        public PositionSizer(double forecast, double maximumRisk, double accountBalance, double lotSize, double pipSize, double stopLossPrice, double entryPrice)
        {
            if (maximumRisk > 0.5)
            {
                throw new Exception("Maximum Risk is set at over 50% of account balance! check this is correct?");
            }
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
            LotSize = lotSize;
            StopLossPrice = stopLossPrice;
            EntryPrice = entryPrice;
        }        
        public double Calculate()
        {
            double stopLossDistance = EntryPrice - StopLossPrice;
            double pipValue = LotSize * PipSize;
            double positionSize = (AccountBalance * RiskPerTrade) / (stopLossDistance * pipValue);
            return positionSize;
        }
    }

}
