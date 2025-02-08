using Application.BackTest;
using Application.Business.Forecasts;
using Domain.Entities;

namespace Application.Business.BackTest
{
    public class TradingSystemParams
    {
        public TradingSystemParams(double tradingCapital, double targetVolatility, double exchangeRate, DateTime startDate, DateTime endDate, double stopLossPercent, int portfolioId, bool enableLogging,
            List<PortfolioInstrument> portfolioInstruments, List<Instrument> instruments, IForecastHandler handler)
        {
            TradingCapital = tradingCapital;
            TargetVolatility = targetVolatility;
            ExchangeRate = exchangeRate;
            PorfolioInstruments = portfolioInstruments;
            Instruments = instruments;
            StartDate = startDate;
            EndDate = endDate;
            StopLossPercent = stopLossPercent;
            PortfolioId = portfolioId;
            Logger = new Logger(enableLogging);
            ForecastType = handler.GetType().Name.ToString();
        }

        public string ForecastType { get; private set; }
        public Logger Logger { get; private set; }
        public double ExchangeRate { get; set; }
        public double TargetVolatility { get; set; }
        public double TradingCapital { get; set; }
        public List<PortfolioInstrument> PorfolioInstruments { get; set; }
        public List<Instrument> Instruments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StopLossPercent { get; set; }
        public int PortfolioId { get; set; }
    }
}