namespace Application.Business.Risk
{
    public interface IRiskManager
    {
        double AccountBalance { get; }
        double Forecast { get; }
        double RiskPerTrade { get; }
        double PipSize { get; }
        double StopLoss { get; }
        double LotSize { get; }
    }
}