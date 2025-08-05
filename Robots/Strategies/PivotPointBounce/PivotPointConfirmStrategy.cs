using Domain.Entities;
using Application.Business.Indicator;
using Application.Business.Market;
using Application.Business.Calculations;
using Domain.Enums;
using Application.Business.Positioning.Validation;
using Application.Interfaces;
using Application.Business.Positioning.Instructions;

namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointConfirmStrategy : BaseStrategy
    {
        public double StrategySignal { get; private set; }
        public PivotPoints PivotPoints { get; private set; }
        public  double MaximumRisk = 0.02;
        public double thresholdForTrade = 0.5;
        public double confirmingSignalsForecastTreshhold = 0.5;


        public List<Test_Parameter> TestParameters{ get; set;  }


        public PivotPointConfirmStrategy(List<Test_Parameter> test_Parameters)
        {
            base.TestParameters = test_Parameters;
        }
        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {           
            foreach (var marketInfo in marketInfos)
            {
                // TODO:  THIS METHOD NEEDS REVIEW - why isn't it creating any instructions?
                //var confirmingSignalsPositidve = false;
                //if (marketInfo.Signals.Count == 0)
                //    confirmingSignalsPositive = true; // no confirming signal filter
                //if (marketInfo.Signals.Count > 0
                //    && ForecastExceedsMinimumThreshold(marketInfo.Signals.AggregatedForecast, confirmingSignalsForecastTreshhold))
                    CalculateNewInstructions(marketInfo, marketInfos);
            }
            return _positionInstructions;
        }
        public void LoadDefaultParameters(Dictionary<string, string> parameters)
        {
            TestParameters = new List<Test_Parameter>();
            if (parameters == null)
            {
                LogMessages.Add("Input parameters dictionary is null. Using hardcoded default values.");
                TestParameters.Add(new Test_Parameter() { Name = "MaximumRisk[Double]", Value = MaximumRisk.ToString() });
                TestParameters.Add(new Test_Parameter() { Name = "ThresholdForTrade[Double]", Value = thresholdForTrade.ToString() });
                TestParameters.Add(new Test_Parameter() { Name = "ConfirmingSignalsForecastTreshhold[Double]", Value = confirmingSignalsForecastTreshhold.ToString() });
            }
            else
            {
                foreach (var paramEntry in parameters)
                {
                    TestParameters.Add(new Test_Parameter() { Name = paramEntry.Key, Value = paramEntry.Value });
                }
                LogMessages.Add($"Parameters loaded from input dictionary. Total entries: {TestParameters.Count}");
            }
        }
        private void CalculateNewInstructions(IMarketInfo marketInfo, List<IMarketInfo> marketInfos)
        {
            PivotPoints = GetPivotPointData(marketInfo.TickTimeFrame, marketInfos);
            if (PivotPoints != null)
            {
                StrategySignal = CaculateStrategySignal(marketInfo);
                if (ForecastExceedsMinimumThreshold(StrategySignal, thresholdForTrade))
                {
                    var stopLoss = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Support2 - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Resistance2);
                    var pricePoint = StrategySignal > 0 ? marketInfo.Ask : marketInfo.Bid;
                    var positionSize = new StopLossBasedPositionSizer(StrategySignal, MaximumRisk, marketInfo.CurrentCapital, marketInfo.PipSize, marketInfo.LotSize, stopLoss, pricePoint).Calculate();
                    var position = new Position()
                    {
                        SymbolName = marketInfo.SymbolName,
                        PositionType = StrategySignal > 0 ? PositionType.BUY : PositionType.SELL,
                        EntryPrice = StrategySignal > 0 ? marketInfo.Ask : marketInfo.Bid,
                        StopLoss = stopLoss,
                        TakeProfit = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Pivot - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Pivot),
                        Volume = positionSize,
                        Created = marketInfo.CursorDate,
                        ExpirationDate = new DateTime(marketInfo.CursorDate.Year, marketInfo.CursorDate.Month, marketInfo.CursorDate.Day, 23, 0, 0)
                    };
                    _positionInstructions.Add(new OpenInstruction(position, GetValidationService()));
                }
            }
            else
            {
                LogMessages.Add($"PivotPoints not found for {marketInfo.SymbolName}");
            }
        }

        private PivotPoints GetPivotPointData(TimeFrame timeFrame, List<IMarketInfo> marketInfos)
        {
            var pivotTimeFrame = GetPivotTimeFrame(timeFrame);
            if (marketInfos.Any(x => x.TickTimeFrame == pivotTimeFrame))
            {
                var pivotMarketInfo = marketInfos.First(x => x.TickTimeFrame == pivotTimeFrame);
                if (pivotMarketInfo.Bars.Count > 0)
                {
                    var pivotPoint = new PivotPoints(pivotMarketInfo.Bars[0].Date, pivotMarketInfo.Bars[0].HighPrice, pivotMarketInfo.Bars[0].LowPrice, pivotMarketInfo.Bars[0].ClosePrice);
                    return pivotPoint;
                }
                else
                {
                    LogMessages.Add($"No bars found for {pivotTimeFrame}");
                    return null;
                }
            }
            else
            {
                LogMessages.Add($"No market info found for {pivotTimeFrame}");
                return null;
            }
        }

        private TimeFrame GetPivotTimeFrame(TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.M1:
                    return TimeFrame.H1;
                case TimeFrame.M5:
                    return TimeFrame.H1;
                case TimeFrame.M15:
                    return TimeFrame.H1;
                case TimeFrame.M30:
                    return TimeFrame.H4;
                case TimeFrame.H1:
                    return TimeFrame.D1;
                case TimeFrame.H4:
                    return TimeFrame.D1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(timeFrame), timeFrame, null);
            }
        }

        private static bool ForecastExceedsMinimumThreshold(double forecast, double thresholdForTrade)
        {
            return forecast >= thresholdForTrade || forecast <= thresholdForTrade * -1;
        }

        private double CalculatePips(double priceDiff)
        {
            var powered = Math.Pow(priceDiff, 2);
            return Math.Sqrt(powered) * 10000;
        }

        private double CaculateStrategySignal(IMarketInfo props)
        {
            var strategySignal = 0.0;
            if(LongConditionMet(props.Bars[0].LowPrice, props.Bars[1].LowPrice))
            {
                strategySignal = 1.0;
            }
            if (ShortConditionMet(props.Bars[0].HighPrice, props.Bars[1].HighPrice))
            {
                strategySignal = -1.0;
            }
            return strategySignal;
        }

        private bool LongConditionMet(double currentPrice, double previousPrice)
        {
            //"Long Entry at Support1";
            return previousPrice < PivotPoints.Support1
                && currentPrice > PivotPoints.Support1;
        }
        private bool ShortConditionMet(double currentPrice, double previousPrice)
        {
            //"Short Entry at Support1";
            return previousPrice > PivotPoints.Resistance1
                && currentPrice < PivotPoints.Resistance1;
        }
    }
}
