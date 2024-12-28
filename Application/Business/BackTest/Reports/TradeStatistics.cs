using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class TradeStatistics
    {
        //TraderDBContextDerived dBContext = new TraderDBContextDerived();
        Test test = new Test();
        int TestId { get; set; }
        public TradeStatistics(int testId)
        {
            TestId = testId;
        }

        public void BuildReport()
        {

            //if (dBContext.Tests.Any(x => x.TestId == TestId))
            //{
            //    test = dBContext.Tests.First(x => x.TestId == TestId);
            //    var trades = test.Test_Trades;
            //    test.GrossProfit = test.Test_Trades.Where(x => x.Margin > 0).Sum(x => x.Margin);
            //    test.GrossLoss = test.Test_Trades.Where(x => x.Margin < 0).Sum(x => x.Margin);
            //    test.TotalTrades = test.Test_Trades.Count;
            //    test.WinningTrades = test.Test_Trades.Where(x => x.Margin > 0).Count();
            //    test.LosingTrades = test.Test_Trades.Where(x => x.Margin < 0).Count();
            //    test.LargestWinningTrade = LargestWinningTrade();
            //    test.AverageTrade = AverageTrade();

            //    test.NetProfit = test.Test_Trades.Sum(x => x.Margin);
            //    test.Commission = test.Test_Trades.Sum(x => x.Commission);
            //    test.NetShortProfit = test.Test_Trades.Where(x => x.Direction == "SELL").Sum(x => x.Margin);
            //    test.NetLongProfit = NetLongProfit();
            //    test.GrossShortProfit = GrossShortProfit();
            //    test.GrossLongProfit = GrossLongProfit();
            //    test.GrossShortLoss = test.Test_Trades.Where(x => x.Direction == "SELL" && x.Margin < 0).Sum(x => x.Margin);
            //    test.GrossLongLoss = GrossLongLoss();
            //    test.ProfitFactor = ProfitFactor();
            //    test.ProfitFactorLongTrades = ProfitFactorLongTrades();
            //    test.ProfitFactorShortTrades = ProfitFactorShortTrades();
            //    test.ProfitableTradesRatio = ProfitableTradesRatio();
            //    test.LosingTradesRatio = LosingTradesRatio();
            //    test.ProfitableLongTradesRatio = ProfitableLongTradesRatio();
            //    test.ProfitableShortTradesRatio = ProfitableShortTradesRatio();
            //    test.AverageWin = AverageWin();
            //    test.AverageWinLong = AverageWinLong();
            //    test.AverageWinShort = AverageWinShort();
            //    test.AverageLoss = AverageLoss();
            //    test.AverageLossLong = AverageLossLong();
            //    test.AverageLossShort = AverageLossShort();
            //    test.MaxAdverseExcursion = MaxAdverseExcursion();
            //    test.SharpeRatio = Convert.ToDouble(new SharpeRatio(test.Test_Trades.ToList()).Get);

            //    dBContext.Tests.AddOrUpdate();
            //    dBContext.SaveChanges();
            //}
            //else
            //    throw new Exception("No test found with TestId value " + TestId);
        }

        private double ProfitFactor()
        {
            if (test.GrossLoss == 0)
                return 0;
            return Convert.ToDouble(test.GrossProfit / test.GrossLoss);
        }

        private decimal AverageTrade()
        {
            //if (!test.TestTrades.Any())
            //    return 0;
            return 0;//test.TestTrades.Average(x => x.Margin);
        }

        private decimal LargestWinningTrade()
        {
            //if (!test.TestTrades.Any())
            //    return 0;
            return  0;//test.LargestLosingTrades = test.TestTrades.Max(x => x.Margin);
        }

        public decimal NetLongProfit()
        {
            return 0;// test.TestTrades.Where(x => x.Direction == "BUY").Sum(x => x.Margin);
        }
        public decimal GrossShortProfit()
        {
            return 0;// test.TestTrades.Where(x => x.Direction == "SELL" && x.Margin > 0).Sum(x => x.Margin);
        }
        public decimal GrossLongProfit()
        {
            return 0;// test.TestTrades.Where(x => x.Direction == "BUY" && x.Margin > 0).Sum(x => x.Margin);
        }

        public decimal GrossLongLoss()
        {
            return 0;// test.TestTrades.Where(x => x.Direction == "BUY" && x.Margin < 0).Sum(x => x.Margin);
        }
        public double ProfitFactorLongTrades()
        {
            var glp = GrossLongProfit();
            var gll = GrossLongLoss();
            if (glp == 0 || gll == 0)
                return 0;
            return Convert.ToDouble(GrossLongProfit() / GrossLongLoss());
        }
        public double ProfitFactorShortTrades()
        {
            var gsp = GrossShortProfit();
            var gsl = GrossShortLoss();
            if (gsl == 0)
                return Convert.ToDouble(gsp);
            if (gsp == 0)
                return 0;
            return Convert.ToDouble(gsp / gsl);
        }

        private decimal GrossShortLoss()
        {
            return 0;//test.TestTrades.Where(x => x.Direction == "SELL" && x.Margin < 0).Sum(x => x.Margin);
        }

        public double ProfitableTradesRatio()
        {
            var noOfTrades = 0;//test.TestTrades.ToList().Count;
            var winners = 0;//test.TestTrades.Where(x => x.Margin > 0).ToList().Count;
            if (noOfTrades == 0)
                return 0;
            double ratio = winners / noOfTrades;
            return ratio;
        }
        public double LosingTradesRatio()
        {
            var noOfTrades = 0;//test.TestTrades.ToList().Count;
            var losers = 0;//test.TestTrades.Where(x => x.Margin < 0).ToList().Count;
            if (noOfTrades == 0)
                return 0;
            return losers / noOfTrades;
        }
        public double ProfitableLongTradesRatio()
        {
            var noOfTrades = 0;//test.TestTrades.Where(x => x.Direction == "BUY").ToList().Count;
            var winners = 0;//test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").ToList().Count;
            if (noOfTrades == 0)
                return 0;
            return winners / noOfTrades;
        }
        public double ProfitableShortTradesRatio()
        {
            var noOfTrades = 0;//test.TestTrades.Where(x => x.Direction == "SELL").ToList().Count;
            var winners = 0;//test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").ToList().Count;
            if (noOfTrades == 0)
                return 0;
            return winners / noOfTrades;
        }
        public decimal AverageWin()
        {
            var winningMargin = 0;//test.TestTrades.Where(x => x.Margin > 0).Sum(x => x.Margin);
            var tradesCount = 0;//test.TestTrades.Where(x => x.Margin > 0).ToList().Count;
            if (winningMargin > 0 && tradesCount > 0)
                return winningMargin / tradesCount;
            return 0;
        }
        public decimal AverageWinLong()
        {
            var winningLongs = 0;//test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").Sum(x => x.Margin);
            var noOfWinningLongs = 0;// test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").ToList().Count;
            if (winningLongs > 0 && noOfWinningLongs > 0)
                return winningLongs / noOfWinningLongs;
            return 0;
        }
        public decimal AverageWinShort()
        {
            var winningShorts = 0;//test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").Sum(x => x.Margin);
            var noOfWinningShorts = 0;//test.TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").ToList().Count;
            if (winningShorts > 0 && noOfWinningShorts > 0)
                return winningShorts / noOfWinningShorts;
            return 0;
        }
        public decimal AverageLoss()
        {
            var losingTrade = 0;//test.TestTrades.Where(x => x.Margin < 0).Sum(x => x.Margin);
            var noOfLosingTrades = 0;//test.TestTrades.Where(x => x.Margin < 0).ToList().Count;
            if (losingTrade < 0 && noOfLosingTrades > 0)
                return losingTrade / noOfLosingTrades;
            return 0;
        }
        public decimal AverageLossLong()
        {
            var sumOfLosses = 0;//test.TestTrades.Where(x => x.Margin < 0 && x.Direction == "BUY").Sum(x => x.Margin);
            var noOfLosingTrades = 0;// test.TestTrades.Where(x => x.Margin < 0 && x.Direction == "BUY").ToList().Count;
            if (noOfLosingTrades > 0)
                return sumOfLosses / noOfLosingTrades;
            return 0;
        }
        public decimal AverageLossShort()
        {
            var shortMarginSum = 0;//test.TestTrades.Where(x => x.Margin < 0 && x.Direction == "SELL").Sum(x => x.Margin);
            var shortLossSum = 0;// test.TestTrades.Where(x => x.Margin < 0 && x.Direction == "SELL").ToList().Count;
            if (shortLossSum == 0)
                return shortMarginSum;
            return shortMarginSum / shortLossSum;
        }
        public double MaxAdverseExcursion()
        {
            var capitalAccrual = test.StartingCapital;
            var maxExcursion = 0M;
            //foreach (var trade in test.TestTrades)
            //{
            //    capitalAccrual += trade.Margin;
            //    if (trade.Margin < 0)
            //    {
            //        var tradeExcursion = trade.Margin / capitalAccrual * -1;
            //        if (tradeExcursion > maxExcursion)
            //            maxExcursion = tradeExcursion;
            //    }
            //}
            return Convert.ToDouble(maxExcursion);
        }
    }
}
