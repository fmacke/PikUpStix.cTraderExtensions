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
            return MapBars(historicalData, 1);  /// TODO: 1 is a placeholder for instrumentId
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

    }
}
