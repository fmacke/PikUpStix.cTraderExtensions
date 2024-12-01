//using Application.Business.BackTest.Position;
//using Application.Business.Forecasts;
//using Domain.Entities;
//using Domain.Enums;
//using PikUpStix.Trading.Forecast;
//using System.Text;

//namespace Application.Business.BackTest
//{
//    public class TradeSimulate
//    {
//        //private readonly TraderDBContextDerived db = new TraderDBContextDerived();
//        public List<List<HistoricalData>> HistoricalDataSets { get; set; }
//        private TradingSystemParams Parameters { get; set; }
//        public int TestId { get; set; }

//        public TradeSimulate(TradingSystemParams parameters)
//        {
//            Parameters = parameters;
//            var instruments = InstrumentList();
//            //UpdateHistoricalData(instruments);
//            HistoricalDataSets = LoadHistoricalDataFromDb();

//            var newTest = new Test
//            {
//                FromDate = Parameters.StartDate,
//                ToDate = Parameters.EndDate,
//                EndingCapital = Convert.ToDecimal(0.0),
//                StartingCapital = Parameters.TradingCapital,
//                TestRunAt = DateTime.Now,
//                Description = GetDescription(Parameters.TargetVolatility, Parameters.StopLossPercent, Parameters.PortfolioId, instruments, Parameters.ForecastType)
//            };
//           // db.Tests.Add(newTest);
//            //db.SaveChanges();
//            TestId = newTest.Id;
//        }

//        public void RunTradeSimulation(IStopLossHandler stopOutStrategy, IForecastHandler forecastStrategy, IPositionHandler positionHandler,
//            IStopLossCreator stopLossCreator)
//        {
//            //DateTime cursorDate = Parameters.StartDate;
//            //var trades = db.Test_Trades.Where(x => x.TestId == TestId && x.Status == PositionStatus.POSITION.ToString()).ToList();

//            //var historicalPositions = new List<Test_Trades>();

//            //while (cursorDate <= Parameters.EndDate)
//            //{
//            //    if (cursorDate.DayOfWeek != DayOfWeek.Saturday && cursorDate.DayOfWeek != DayOfWeek.Sunday)
//            //    {
//            //        Parameters.Logger.LogNewDay(cursorDate);
//            //        try
//            //        {
//            //            // 1. Close any stopped out positions
//            //            trades = stopOutStrategy.CloseStoppedOutPositions(trades, cursorDate, HistoricalDataSets, Parameters.ExchangeRate, Parameters.TradingCapital);

//            //            double askingPrice = 0;
//            //            double biddingPrice = 0;  //yep, just needed build errors removed during addition of bp and ap to system in lieu of 'currentPrice'
//            //            // 2. Calculate Today's forecasts
//            //            var instrumentForecasts = forecastStrategy.GetForecasts(HistoricalDataSets, cursorDate, Parameters.Logger, askingPrice, biddingPrice, new List<Test_Parameters>());

//            //            // 3. Calculate today's proposed positions accounting for portfolio weighting
//            //            var margin = trades.Sum(x => x.Margin) + Parameters.TradingCapital;
//            //            var weightedPositions = new WeightedProposedPositions(instrumentForecasts, Parameters.StopLossPercent, Parameters.ExchangeRate, Parameters.TargetVolatility, HistoricalDataSets, margin);

//            //            // 4. Adjust Positions According to new forecasts
//            //            trades = positionHandler.GetUpdatedPositions(Parameters, TestId, trades, margin, cursorDate,
//            //                weightedPositions, HistoricalDataSets, stopLossCreator);
//            //        }
//            //        catch (Exception ex)
//            //        {
//            //            Parameters.Logger.MethodError("TradeSimulate.RunTradeSimulation", "Error in test run at " + cursorDate + ": " + ex.Message);
//            //        }
//            //    }
//            //    cursorDate = cursorDate.AddDays(1);
//            //}
//            //UpdateTestResultsNoDB(trades);
//        }

//        private void UpdateHistoricalData(List<int> instrumentIdLists)
//        {
//            //var quandlDataImporter = new QuandlDataImport();
//            //var pullDataFrom = new DateTime(1974, 1, 1);
//            //quandlDataImporter.ImportLatestData(db.Instruments.Where(x => instrumentIdLists.Contains(x.InstrumentId)).ToList(), pullDataFrom);
//        }

//        private List<int> InstrumentList()
//        {
//            var instrumentList = new List<int>();
//            foreach (PortfolioInstrument instrument in Parameters.PorfolioInstruments)
//                instrumentList.Add(instrument.InstrumentId);
//            return instrumentList;
//        }

//        private void UpdateTestResultsNoDB(List<Test_Trades> trades)
//        {
//            //Test test = db.Tests.First(x => x.TestId == TestId);
//            //test.StartingCapital = Parameters.TradingCapital;
//            //test.TestEndAt = DateTime.Now;
//            //test.EndingCapital = trades.Sum(x => x.Margin) + Parameters.TradingCapital;
//            //test.Test_AnnualReturns = new AnnualReturns(TestId, test.Test_Trades);
//            //foreach (var trade in trades)
//            //    test.Test_Trades.Add(trade);
//            ////test.MaxAdverseExcursion = new DailyExcursions(test.Test_Trades.ToList()).MaxAdverseExcursion;
//            ////test.SharpeRatio = new SharpeRatio(test.Test_Trades.ToList()).Get;
//            //db.Tests.AddOrUpdate();
//            //db.SaveChanges();
//        }

//        private string GetDescription(decimal targetVolatility, decimal stopLoss, decimal portfolioId, List<int> instrumentIds, string forecastHandler)
//        {
//            var simulationDescription = new StringBuilder();
//            simulationDescription.Append("ForecastHander: " + forecastHandler + "|| ");
//            simulationDescription.Append("TargetVolatility: " + targetVolatility + "|| ");
//            simulationDescription.Append("StopLoss: " + stopLoss + "|| ");
//            simulationDescription.Append("PorfolioId: " + portfolioId + "|| ");
//            IQueryable<Instrument> instruments = db.Instruments.Where(x => instrumentIds.Contains(x.InstrumentId));
//            foreach (Instrument instrument in instruments)
//                simulationDescription.Append(instrument.InstrumentName + "(" + instrument.Id + ")  ||  ");
//            return simulationDescription.ToString();
//        }

//        private List<List<HistoricalData>> LoadHistoricalDataFromDb()
//        {
//            //var hds = new List<List<HistoricalData>>();
//            //foreach (PortfolioInstrument instrument in Parameters.PorfolioInstruments)
//            //{
//            //    var data = db.HistoricalDatas.Where(
//            //            x =>
//            //                x.InstrumentId == instrument.InstrumentId && x.Date >= Parameters.StartDate &&
//            //                x.Date <= Parameters.EndDate).ToList();
//            //    foreach (var i in data)
//            //    {
//            //        // Remove time refernce leaving only date - system can currenly only deal with a time value of 00:00:00
//            //        i.Date = new DateTime(i.Date.Value.Year, i.Date.Value.Month, i.Date.Value.Day);
//            //    }
//            //    hds.Add(data);
//            //}
//            //return hds;
//            return new List<List<HistoricalData>>();
//        }
//    }
//}