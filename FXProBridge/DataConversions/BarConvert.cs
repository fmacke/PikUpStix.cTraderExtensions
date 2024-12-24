using AutoMapper;
using cAlgo.API;
using DataServices;
using Domain.Entities;
using Application.Mappings;

namespace FXProBridge.DataConversions
{
    public static class BarConvert
    {
        public static List<HistoricalData> GetHistoData(Bars historicalData, Instrument instrument)
        {
            var histoData = new List<HistoricalData>();
            return MapBars(historicalData, instrument.Id);
        }
        public static List<HistoricalData> GetHistoData(Bars historicalData)
        {
            var instrument = GetInstrumentDetails(historicalData.SymbolName);
            return MapBars(historicalData, instrument.Id);
        }

        private static List<HistoricalData> MapBars(Bars historicalData, int instrumentId)
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
                    //Instrument = instrument,
                    Date = historicalData.OpenTimes[x],
                    //InstrumentId = instrumentId
                });
            }
            return histoData;
        }

        private static Instrument GetInstrumentDetails(string symbolName)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.AddProfile<InstrumentProfile>());
                var mapper = config.CreateMapper();
                var dataService = new DataService();
                var DD = new DataService().Instruments.GetAllInstrumentsCachedAsync();
                var single = DD.First(x => x.DataName == symbolName && x.DataSource == "FXPRO");
                var instrument = mapper.Map<Instrument>(single); ;
                return instrument;
            }
            catch (Exception ex)
            {
                throw new Exception("Problem retrieving instrument data - it may not exist in sql server", ex);
            }
        }
    }
}
