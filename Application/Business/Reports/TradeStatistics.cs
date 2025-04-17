using Application.Business.Calculations;
using Application.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Reports
{
    public class TradeStatistics
    {
        public List<Position> Positions { get; private set; } = new List<Position>();
        public double GrossProfit { get {
                return Positions.Where(x => x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossLoss { get {
                return Positions.Where(x => x.Margin < 0).Sum(x => x.Margin)*-1;
            }}
        public int TotalTrades { get {
                return Positions.Count;
            }}
        public int WinningTrades { get {
            return Positions.Where(x => x.Margin > 0).Count();
            }}
        public int LosingTrades { get {
                return Positions.Where(x => x.Margin < 0).Count();
            }}
        public double LargestWinningTrade { get {
                if (!Positions.Any())
                    return 0;
                return Positions.Max(x => x.Margin);
            }}
        public double LargestLosingTrades{ get{
                if (!Positions.Any())
                    return 0;
                return Positions.Min(x => x.Margin);
            }}
        public double AverageTrade { get {
                if (!Positions.Any())
                    return 0;
                return Positions.Average(x => x.Margin);
            }}
        public double NetProfit { get {
            return Positions.Sum(x => x.Margin);
            }}
        public double Commission { get {
            return Positions.Sum(x => x.Commission);
            }}
        public double NetShortProfit { get {
               return Positions.Where(x => x.PositionType == PositionType.SELL).Sum(x => x.Margin);
            }}
        public double NetLongProfit { get {
                return Positions.Where(x => x.PositionType == PositionType.BUY).Sum(x => x.Margin);
            }}
        public double GrossShortProfit { get {
                return Positions.Where(x => x.PositionType == PositionType.SELL && x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossLongProfit { get {
                return Positions.Where(x => x.PositionType == PositionType.BUY && x.Margin > 0).Sum(x => x.Margin);
            }}
        public double GrossShortLoss { get {
                return Positions.Where(x => x.PositionType == PositionType.SELL && x.Margin < 0).Sum(x => x.Margin);
            }
        }
        public double GrossLongLoss { get {
                return Positions.Where(x => x.PositionType == PositionType.BUY && x.Margin < 0).Sum(x => x.Margin);
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
                var noOfTrades = Positions.ToList().Count;
                var winners = Positions.Where(x => x.Margin > 0).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                double ratio = winners / noOfTrades;
                return ratio;
            }}
        public double LosingTradesRatio { get {
                var noOfTrades = Positions.ToList().Count;
                var losers = Positions.Where(x => x.Margin < 0).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return losers / noOfTrades;
            }}
        public double ProfitableLongTradesRatio { get{
                var noOfTrades = Positions.Where(x => x.PositionType == PositionType.BUY).ToList().Count;
                var winners = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.BUY).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return winners / noOfTrades;
            }}
        public double ProfitableShortTradesRatio { get{
                var noOfTrades = Positions.Where(x => x.PositionType == PositionType.SELL).ToList().Count;
                var winners = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.SELL).ToList().Count;
                if (noOfTrades == 0)
                    return 0;
                return winners / noOfTrades;
            }}
        public double AverageWin { get{
                var winningMargin = Positions.Where(x => x.Margin > 0).Sum(x => x.Margin);
                var tradesCount = Positions.Where(x => x.Margin > 0).ToList().Count;
                if (winningMargin > 0 && tradesCount > 0)
                    return winningMargin / tradesCount;
                return 0;
            }}
        public double AverageWinLong { get{
                var winningLongs = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.BUY).Sum(x => x.Margin);
                var noOfWinningLongs = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.BUY).ToList().Count;
                if (winningLongs > 0 && noOfWinningLongs > 0)
                    return winningLongs / noOfWinningLongs;
                return 0;
            }}
        public double AverageWinShort { get{
                var winningShorts = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.SELL).Sum(x => x.Margin);
                var noOfWinningShorts = Positions.Where(x => x.Margin > 0 && x.PositionType == PositionType.SELL).ToList().Count;
                if (winningShorts > 0 && noOfWinningShorts > 0)
                    return winningShorts / noOfWinningShorts;
                return 0;
            }}
        public double AverageLoss { get{
                var losingTrade = Positions.Where(x => x.Margin < 0).Sum(x => x.Margin);
                var noOfLosingTrades = Positions.Where(x => x.Margin < 0).ToList().Count;
                if (losingTrade < 0 && noOfLosingTrades > 0)
                    return losingTrade / noOfLosingTrades;
                return 0;
            }}
        public double AverageLossLong { get{
                var sumOfLosses = Positions.Where(x => x.Margin < 0 && x.PositionType == PositionType.BUY).Sum(x => x.Margin);
                var noOfLosingTrades = Positions.Where(x => x.Margin < 0 && x.PositionType == PositionType.BUY).ToList().Count;
                if (noOfLosingTrades > 0)
                    return sumOfLosses / noOfLosingTrades;
                return 0;
            }}
        public double AverageLossShort { get {
                var shortMarginSum = Positions.Where(x => x.Margin < 0 && x.PositionType == PositionType.SELL).Sum(x => x.Margin);
                var shortLossSum = Positions.Where(x => x.Margin < 0 && x.PositionType == PositionType.SELL).ToList().Count;
                if (shortLossSum == 0)
                    return shortMarginSum;
                return shortMarginSum / shortLossSum;
            }}
        public double MaxAdverseExcursion { get; private set; } = 0.0;
        public double StartingAccountBalance { get; private set; } = 0.0;
        public double SharpeRatio { get {
                return new SharpeRatio(Positions.ToList()).Value;
            }}

        public double MaxBalanceDrawdown
        {
            get
            {
                double peakBalance = StartingAccountBalance;
                double maxDrawdown = 0.0;
                double balance = StartingAccountBalance;

                foreach (var trade in Positions.OrderBy(t => t.ClosedAt))
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
                foreach (var trade in Positions.OrderBy(t => t.ClosedAt))
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

                foreach (var trade in Positions)
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

                foreach (var trade in Positions)
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
                var returns = Positions.Select(t => t.Margin).ToList();
                if (returns.Count == 0) return 0;
                double averageReturn = returns.Average();
                double downsideDeviation = CalculateDownsideDeviation(returns);

                if (downsideDeviation == 0)
                    return double.PositiveInfinity;

                return (averageReturn - RiskFreeRate) / downsideDeviation;
            }
        }
        public double NetShortLoss
        {
            get
            {
                return Positions.Where(t => t.Margin < 0 && t.PositionType == PositionType.SELL).Sum(t => t.Margin);
            }
        }
        public double NetLongLoss
        {
            get
            {
                return Positions.Where(t => t.PositionType == PositionType.BUY && t.Margin < 0).Sum(t => t.Margin);
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


        public TradeStatistics(List<Position> testTrades, double startingAccountBalance, double maximumAdverseExcursion)
        {
            Positions = testTrades.Where(t => t.ClosedAt != null).ToList();
            MaxAdverseExcursion = maximumAdverseExcursion;
            StartingAccountBalance = startingAccountBalance;
            RiskFreeRate = 2.0;
        }
    }
}
