using cAlgo.API;
using Domain.Entities;
using Infrastructure.Contexts;

namespace Robots.Common
{
    public class BarConvert
    /// <summary>
    /// Converts cAlgo.API historical bar data into a list of PikUpStix.Domain.Entities.HistoricalData objects.
    /// It provides methods to retrieve historical data either by directly passing an Instrument or by fetching 
    /// the Instrument details based on the symbol name.
    /// Where instrument data is not provided, a database context is used to fetch Instrument details and processes 
    /// the bar data to create HistoricalData objects.
    /// </summary>
    {
        public Instrument Instrument { get; set; }
        public List<HistoricalData> GetHistoData(Bars historicalData, Instrument instrument)
        {
            var histoData = new List<HistoricalData>();
            return LoadData(historicalData, instrument);
        }
        public List<HistoricalData> GetHistoData(Bars historicalData)
        {
            return LoadData(historicalData, GetInstrumentDetails(historicalData.SymbolName));
        }

        private List<HistoricalData> LoadData(Bars historicalData, Instrument instrument)
        {
            var histoData = new List<HistoricalData>();
            for (var x = historicalData.ClosePrices.Count - 1; x >= 0; x--)
            {
                histoData.Add(new HistoricalData()
                {
                    ClosePrice = Convert.ToDecimal(historicalData.ClosePrices[x]),
                    OpenPrice = Convert.ToDecimal(historicalData.OpenPrices[x]),
                    HighPrice = Convert.ToDecimal(historicalData.HighPrices[x]),
                    LowPrice = Convert.ToDecimal(historicalData.LowPrices[x]),
                    Instrument = instrument,
                    Date = historicalData.OpenTimes[x],
                    InstrumentId = instrument.InstrumentId
                });
            }
            return histoData;
        }

        private Instrument GetInstrumentDetails(string symbolName)
        {
            try
            {
                Instrument = new ApplicationDbContext().Instruments.First(x => x.DataName == symbolName && x.DataSource == "FXPRO");
                return Instrument;
            }
            catch (Exception ex)
            {
                throw new Exception("Problem retrieving instrument data - it may not exist in sql server", ex);
            }
        }
    }
}
