using Domain.Entities;
using System.ComponentModel;

namespace Application.Business.Forecasts
{
    public abstract class ForecastValue : IForecastValue
    {
        public ForecastValue()
        {
        }
        public ForecastValue(DateTime date, List<HistoricalData> historicalData, double askingPrice, double biddingPrice)
        {
            // TraderDBContextDerived dbx = new TraderDBContextDerived();
            //  var id = historicalData.First().InstrumentId;
            //var insId = dbx.Instruments.First(x => x.InstrumentId == id).InstrumentId;
            Instrument = new Instrument()
            {
                ContractUnit = 1,
                InstrumentName = "EURUSD",
                Id = 1,
                Currency = "GBP",
                MinimumPriceFluctuation = 0.0001
            };
            DateTime = date;
            PriceData = historicalData;
            AskingPrice = askingPrice;
            BiddingPrice = biddingPrice;
            InstrumentBlock = Instrument.ContractUnit;
            MinimumPriceFluctuation =Instrument.MinimumPriceFluctuation;
        }

        public List<HistoricalData> PriceData { get; protected set; }
        public double AskingPrice { get; set; }
        public DateTime DateTime { get; set; }
        public double Forecast { get; set; }
        public double BiddingPrice { get; set; }
        public double InstrumentBlock { get; set; }
        public double ShortForecast { get; protected set; }
        public double LongForecast { get; protected set; }
        public double MediumForecast { get; protected set; }
        public Instrument Instrument { get; set; }
        public double MinimumPriceFluctuation { get; set; }

        public double CalculateForecast()
        {
            throw new Exception("implement in child classes");
        }

        //protected double CalculateCurrentPrice(List<HistoricalData> historicalPriceSet, DateTime cursorDate)
        //{
        //    if (historicalPriceSet.Any(x => Convert.ToDateTime(x.Date).Date == cursorDate.Date))
        //        return
        //            Convert.ToDecimal(historicalPriceSet.First(x => Convert.ToDateTime(x.Date).Date == cursorDate.Date)
        //                .ClosePrice);
        //    return 0;
        //}

        //protected bool EnoughHistoryToCalculateForecast(int index, int maxForecastLength, int maxCount)
        //{
        //    return index + maxForecastLength < maxCount;
        //}

        //protected bool DateIsInRange(int index)
        //{
        //    return index >= 0;
        //}
    }
}