using DocumentFormat.OpenXml.Office.CoverPageProps;
using Domain.Entities;

namespace Application.Business.Forecasts
{
    public abstract class ForecastValue : IForecastValue
    {
        public List<HistoricalData> PriceData { get; protected set; }
        public double AskingPrice { get; set; }
        public DateTime DateTime { get; set; }
        public double Forecast { get; set; }
        public double BiddingPrice { get; set; }
        public double InstrumentBlock { get; set; }
        public double ShortForecast { get; protected set; }
        public string SymbolName { get; set; }
        public double LongForecast { get; protected set; }
        public double MediumForecast { get; protected set; }
        public Instrument Instrument { get; set; }
        public double MinimumPriceFluctuation { get; set; }

        public ForecastValue()
        {
        }
        public ForecastValue(IMarketInfo marketInfo)
        {
            Instrument = new Instrument()
            {
                ContractUnit = 1,
                InstrumentName = marketInfo.SymbolName,
                Id = 1,
                Currency = marketInfo.Currency,
                MinimumPriceFluctuation = 0.0001
            };
            DateTime = marketInfo.CursorDate;
            PriceData = marketInfo.Bars;
            AskingPrice = marketInfo.Ask;
            BiddingPrice = marketInfo.Bid;
            SymbolName = marketInfo.SymbolName;
            InstrumentBlock = Instrument.ContractUnit;
            MinimumPriceFluctuation = Instrument.MinimumPriceFluctuation;
        }

        public double CalculateForecast()
        {
            throw new Exception("implement in child classes");
        }
    }
}