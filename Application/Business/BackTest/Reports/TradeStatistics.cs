using Application.Mappings;
using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class TradeStatistics
    {
        public List<TestTrade> TestTrades { get; private set; } = new List<TestTrade>();
        public double GrossProfit { get {
                return TestTrades.Where(x => x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossLoss { get {
                return TestTrades.Where(x => x.Margin < 0).Sum(x => x.Margin)*-1;
            }}
        public int TotalTrades { get {
                return TestTrades.Count;
            }}
        public int WinningTrades { get {
            return TestTrades.Where(x => x.Margin > 0).Count();
            }}
        public int LosingTrades { get {
                return TestTrades.Where(x => x.Margin < 0).Count();
            }}
        public double LargestWinningTrade { get {
                if (!TestTrades.Any())
                    return 0;
                return TestTrades.Max(x => x.Margin);
            }}
        public double LargestLosingTrades{ get{
                if (!TestTrades.Any())
                    return 0;
                return TestTrades.Min(x => x.Margin);
            }}
        public double AverageTrade { get {
                if (!TestTrades.Any())
                    return 0;
                return TestTrades.Average(x => x.Margin);
            }}
        public double NetProfit { get {
            return TestTrades.Sum(x => x.Margin);
            }}
        public double Commission { get {
            return TestTrades.Sum(x => x.Commission);
            }}
        public double NetShortProfit { get {
               return TestTrades.Where(x => x.Direction == "SELL").Sum(x => x.Margin);
            }}
        public double NetLongProfit { get {
                return TestTrades.Where(x => x.Direction == "BUY").Sum(x => x.Margin);
            }}
        public double GrossShortProfit { get {
                return TestTrades.Where(x => x.Direction == "SELL" && x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossLongProfit { get {
                return TestTrades.Where(x => x.Direction == "BUY" && x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossShortLoss { get {
                return TestTrades.Where(x => x.Direction == "SELL" && x.Margin < 0).Sum(x => x.Margin);
            }
        }
        public double GrossLongLoss { get {
                return TestTrades.Where(x => x.Direction == "BUY" && x.Margin < 0).Sum(x => x.Margin);
            }}
        public double ProfitFactor { get{
                if (GrossLoss == 0)
                    return 0;
                return Convert.ToDouble(GrossProfit / GrossLoss);
            }}
        public double ProfitFactorLongTrades { get {
                var glp = GrossLongProfit;
                var gll = GrossLongLoss;
                if (glp == 0 || gll == 0)
                    return 0;
                return GrossLongProfit / GrossLongLoss;
            }}
        public double ProfitFactorShortTrades { get
            {
                var gsp = GrossShortProfit;
                var gsl = GrossShortLoss;
                if (gsl == 0)
                    return gsp;
                if (gsp == 0)
                    return 0;
                return gsp / gsl;
            }}
        public double ProfitableTradesRatio { get{
                var noOfTrades = TestTrades.ToList().Count;
                var winners = TestTrades.Where(x => x.Margin > 0).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                double ratio = winners / noOfTrades;
                return ratio;
            }}
        public double LosingTradesRatio { get {
                var noOfTrades = TestTrades.ToList().Count;
                var losers = TestTrades.Where(x => x.Margin < 0).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return losers / noOfTrades;
            }}
        public double ProfitableLongTradesRatio { get{
                var noOfTrades = TestTrades.Where(x => x.Direction == "BUY").ToList().Count;
                var winners = TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return winners / noOfTrades;
            }}
        public double ProfitableShortTradesRatio { get{
                var noOfTrades = TestTrades.Where(x => x.Direction == "SELL").ToList().Count;
                var winners = TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return winners / noOfTrades;
            }}
        public double AverageWin { get{
                var winningMargin = TestTrades.Where(x => x.Margin > 0).Sum(x => x.Margin);
                var tradesCount = TestTrades.Where(x => x.Margin > 0).ToList().Count;
                if (winningMargin > 0 && tradesCount > 0)
                    return winningMargin / tradesCount;
                return 0;
            }}
        public double AverageWinLong { get{
                var winningLongs = TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").Sum(x => x.Margin);
                var noOfWinningLongs = TestTrades.Where(x => x.Margin > 0 && x.Direction == "BUY").ToList().Count;
                if (winningLongs > 0 && noOfWinningLongs > 0)
                    return winningLongs / noOfWinningLongs;
                return 0;
            }}
        public double AverageWinShort { get{
                var winningShorts = TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").Sum(x => x.Margin);
                var noOfWinningShorts = TestTrades.Where(x => x.Margin > 0 && x.Direction == "SELL").ToList().Count;
                if (winningShorts > 0 && noOfWinningShorts > 0)
                    return winningShorts / noOfWinningShorts;
                return 0;
            }}
        public double AverageLoss { get{
                var losingTrade = TestTrades.Where(x => x.Margin < 0).Sum(x => x.Margin);
                var noOfLosingTrades = TestTrades.Where(x => x.Margin < 0).ToList().Count;
                if (losingTrade < 0 && noOfLosingTrades > 0)
                    return losingTrade / noOfLosingTrades;
                return 0;
            }}
        public double AverageLossLong { get{
                var sumOfLosses = TestTrades.Where(x => x.Margin < 0 && x.Direction == "BUY").Sum(x => x.Margin);
                var noOfLosingTrades = TestTrades.Where(x => x.Margin < 0 && x.Direction == "BUY").ToList().Count;
                if (noOfLosingTrades > 0)
                    return sumOfLosses / noOfLosingTrades;
                return 0;
            }}
        public double AverageLossShort { get {
                var shortMarginSum = TestTrades.Where(x => x.Margin < 0 && x.Direction == "SELL").Sum(x => x.Margin);
                var shortLossSum = TestTrades.Where(x => x.Margin < 0 && x.Direction == "SELL").ToList().Count;
                if (shortLossSum == 0)
                    return shortMarginSum;
                return shortMarginSum / shortLossSum;
            }}
        public double MaxAdverseExcursion { get; private set; }
        public double StartingAccountBalance { get; private set; }
        public double SharpeRatio { get {
                return new SharpeRatio(TestTrades.ToList()).Value;
            }}

        public double MaxBalanceDrawdown
        {
            get
            {
                double peakBalance = StartingAccountBalance;
                double maxDrawdown = 0.0;
                double balance = StartingAccountBalance;

                foreach (var trade in TestTrades.OrderBy(t => t.ClosedAt))
                {
                    balance += trade.Margin;
                    if (balance > peakBalance)
                        peakBalance = balance;
                    double drawdown = (peakBalance - balance) / peakBalance;
                    if (drawdown > maxDrawdown)
                        maxDrawdown = drawdown;
                }
                return maxDrawdown * 100; // Return as a percentage
            }
        }
        public double MaxEquityDrawdown
        {
            get
            {
                double peakEquity = StartingAccountBalance;
                double maxDrawdown = 0.0;
                double equity = StartingAccountBalance;
                foreach (var trade in TestTrades.OrderBy(t => t.ClosedAt))
                {
                    equity += trade.Margin;
                    if (equity > peakEquity)
                        peakEquity = equity;
                    double drawdown = (peakEquity - equity) / peakEquity;
                    if (drawdown > maxDrawdown)
                        maxDrawdown = drawdown;
                }
                return maxDrawdown * 100; // Return as a percentage
            }
        }

        public int MaxConsecutiveLosingTrades
        {
            get
            {
                int maxConsecutiveLosingTrades = 0;
                int currentConsecutiveLosingTrades = 0;

                foreach (var trade in TestTrades)
                {
                    if (trade.Margin < 0)
                    {
                        currentConsecutiveLosingTrades++;
                        if (currentConsecutiveLosingTrades > maxConsecutiveLosingTrades)
                        {
                            maxConsecutiveLosingTrades = currentConsecutiveLosingTrades;
                        }
                    }
                    else
                    {
                        currentConsecutiveLosingTrades = 0;
                    }
                }

                return maxConsecutiveLosingTrades;
            }
        }
        public int MaxConsecutiveWinningTrades
        {
            get
            {
                int maxConsecutiveWinningTrades = 0;
                int currentConsecutiveWinningTrades = 0;

                foreach (var trade in TestTrades)
                {
                    if (trade.Margin > 0)
                    {
                        currentConsecutiveWinningTrades++;
                        if (currentConsecutiveWinningTrades > maxConsecutiveWinningTrades)
                        {
                            maxConsecutiveWinningTrades = currentConsecutiveWinningTrades;
                        }
                    }
                    else
                    {
                        currentConsecutiveWinningTrades = 0;
                    }
                }

                return maxConsecutiveWinningTrades;
            }
        }
        public double SortinoRatio
        {
            get
            {
                var returns = TestTrades.Select(t => t.Margin).ToList();
                double averageReturn = returns.Average();
                double downsideDeviation = CalculateDownsideDeviation(returns);

                if (downsideDeviation == 0)
                {
                    return double.PositiveInfinity;
                }

                return (averageReturn - RiskFreeRate) / downsideDeviation;
            }
        }
        public double NetShortLoss
        {
            get
            {
                return TestTrades.Where(t => t.Margin < 0 && t.Direction == "SELL").Sum(t => t.Margin);
            }
        }
        public double NetLongLoss
        {
            get
            {
                return TestTrades.Where(t => t.Direction == "BUY" && t.Margin < 0).Sum(t => t.Margin);
            }
        }
        public double RiskFreeRate { get; private set; }
        private double CalculateDownsideDeviation(List<double> returns)
        {
            double averageReturn = returns.Average();
            var downsideReturns = returns.Where(r => r < averageReturn).ToList();

            if (downsideReturns.Count == 0)
            {
                return 0;
            }

            double sumOfSquaredDifferences = downsideReturns.Sum(r => Math.Pow(r - averageReturn, 2));
            return Math.Sqrt(sumOfSquaredDifferences / downsideReturns.Count);
        }


        public TradeStatistics(List<TestTrade> testTrades, double startingAccountBalance, double maximumAdverseExcursion)
        {
            TestTrades = testTrades;
            MaxAdverseExcursion = maximumAdverseExcursion;
            StartingAccountBalance = startingAccountBalance;
            RiskFreeRate = 2.0;
        }
    }
}
