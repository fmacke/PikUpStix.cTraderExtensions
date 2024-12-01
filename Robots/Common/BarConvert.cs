using AutoMapper;
using cAlgo.API;
using DataServices;
using Domain.Entities;
using Application.Mappings;
using Application.Business;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Robots.Common
{
    public static class PositionConvert
    {
        public static Application.Business.Positions ConvertPosition(cAlgo.API.Positions positions)
        {
           var convertedPositions = new Application.Business.Positions();
            foreach (var position in positions)
            {
                convertedPositions.Add(new Application.Business.Position()
                {
                    SymbolName = position.SymbolName,
                    Volume = Convert.ToDecimal(position.VolumeInUnits),
                    StopLoss = Convert.ToDecimal(position.StopLoss),
                    TakeProfit = Convert.ToDecimal(position.TakeProfit),
                    EntryPrice = Convert.ToDecimal(position.EntryPrice),
                    Id = position.Id,
                    TradeType = ConvertTradeType(position.TradeType),
                });
            }
            return convertedPositions;
        }

        private static Domain.Enums.TradeType ConvertTradeType(cAlgo.API.TradeType tradeType)
        {
            if (tradeType == cAlgo.API.TradeType.Buy)
                return Domain.Enums.TradeType.Buy;
            return Domain.Enums.TradeType.Sell;
        }
    }
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
                    InstrumentId = instrumentId
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
