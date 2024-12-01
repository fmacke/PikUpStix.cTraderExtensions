//using System;
//using System.Collections.Generic;
//using System.Linq;
//using PikUpStix.Trading.Common;
//using PikUpStix.Trading.Common.Enums;
//using PikUpStix.Trading.Data.Local.SqlDb;

//namespace PikUpStix.Trading.BackTest
//{
//    public class NewDayPositionAdjustments
//    {
//        //private readonly TraderDbContext db = new TraderDbContext();
//        private Positions ExistingPositions;
//        private decimal CurrentTradingCapital { get; set; }

//        public NewDayPositionAdjustments(decimal exchangeRate, decimal currentTradingCapital, int testId, decimal stopLossPercent, Positions existingPositions, Logger logger)
//        {
//            TestId = testId;
//            ExchangeRate = exchangeRate;
//            StopLossPercent = stopLossPercent;
//            ExistingPositions = existingPositions;
//            CurrentTradingCapital = currentTradingCapital;
//            Logger = logger;
//        }

//        public Logger Logger { get; set; }
//        public decimal StopLossPercent { get; set; }
//        public decimal ExchangeRate { get; private set; }
//        public int TestId { get; private set; }

//        public Positions AdjustmentsUsingNewDate(DateTime cursorDate, List<List<HistoricalData>> historicalDataSets)
//        {
//            //DealWithCloseOuts(cursorDate, historicalDataSets);
//            //UpDateStopLossesAndTrailingStops(cursorDate, historicalDataSets);
//            return ExistingPositions;
//        }
//        private void DealWithCloseOuts(DateTime cursorDate, List<List<HistoricalData>> historicalDataSets)
//        {
//            if (!ExistingPositions.Any()) return;
//            ExistingPositions.CloseStoppedOutPositions(cursorDate);
//            ExistingPositions.CloseOutProfitTargets(cursorDate);
//        }

//        private void UpDateStopLossesAndTrailingStops(DateTime cursorDate, List<List<HistoricalData>> historicalDataSets)
//        {
//            if (!ExistingPositions.Any()) return;
//            ExistingPositions.CalculateCurrentMargins(cursorDate);
//            AdjustStopLosses(cursorDate, historicalDataSets);
//            //AdjustTrailingStops(existingPositions);
            
//        }

//        private void AdjustStopLosses(DateTime cursorDate, List<List<HistoricalData>> historicalDataSets)
//        {
//            foreach (var existingPosition in ExistingPositions)
//            {
//                // Recalculate stop loss based on today's currentPrice, stopLosss percent and currentTrading capital
//                foreach (var historicalDataSet in historicalDataSets)
//                {
//                    var recentHistory = new RecentHistory(historicalDataSets, historicalDataSet.First().InstrumentId,
//                        cursorDate, Logger);
//                    if (!recentHistory.HasValidData) continue;
//                    existingPosition.StopLoss = new StopLoss(CurrentTradingCapital, StopLossPercent,
//                        recentHistory.CurrentTick.Instrument.ContractUnit, GetAggregatePosition(existingPosition),
//                        ExchangeRate,
//                        GetDirection(existingPosition), Convert.ToDecimal(recentHistory.CurrentTick.OpenPrice),
//                        recentHistory.CurrentTick.Instrument.MinimumPriceFluctuation).StopLossInCurrency();
//                }
//            }
//        }

//        private decimal GetAggregatePosition(Test_Trades existingPosition)
//        {
//            return ExistingPositions.Where(x => x.InstrumentId == existingPosition.InstrumentId).Sum(x => x.Volume);
//        }

//        private PositionType GetDirection(Test_Trades existingPosition)
//        {
//            return existingPosition.Volume < 0 ? PositionType.SELL : PositionType.BUY;
//        }
//    }
//}