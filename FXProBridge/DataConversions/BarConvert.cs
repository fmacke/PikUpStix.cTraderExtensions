using cAlgo.API;
using Domain.Entities;

namespace FXProBridge.DataConversions
{
    public static class BarConvert
    {
        public static List<HistoricalData> ConvertBars(Bars historicalData, Instrument instrument)
        {
            var histoData = new List<HistoricalData>();
            return MapBars(historicalData, instrument.Id);
        }
        public static List<HistoricalData> ConvertBars(Bars historicalData)
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
                    ClosePrice = historicalData.ClosePrices[x],
                    OpenPrice = historicalData.OpenPrices[x],
                    HighPrice = historicalData.HighPrices[x],
                    LowPrice = historicalData.LowPrices[x],
                    //Instrument = instrument,
                    Date = historicalData.OpenTimes[x],
                    //InstrumentId = instrumentId
                });
            }
            return histoData;
        }

    }
}
