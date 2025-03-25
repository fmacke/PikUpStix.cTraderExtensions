using Domain.Entities;
using Application.Business.Indicator;
using Application.Business.Indicator.Signal;
using Application.Business.Market;
using Application.Business.Risk;
using Domain.Enums;
using Application.Business.Strategy;
using Application.Business.Positioning;
using Application.Business.Positioning.Validation;
namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointConfirmStrategy : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<IPositionInstruction> PositionInstructions { get; set; } = new List<IPositionInstruction>();
        public double StrategySignal { get; private set; }
        public PivotPoints PivotPoints { get; private set; }
        public  double MaximumRisk = 0.02;
        public IValidationService ValidationService { get; set; } = new ValidationService();

        public PivotPointConfirmStrategy(IMarketInfo props, ConfirmingSignals signals, double thresholdForTrade, 
            double confirmingSignalsForecastTreshhold, PivotPoints pivotPoints, double riskPerTrade)
        {
            MaximumRisk = riskPerTrade;
            var confirmingSignalsPositive = false;
            if (signals.Count == 0)
            {
                confirmingSignalsPositive = true; // no confirming signal filter
            }
            if (signals.Count > 0 && ForecastExceedsMinimumThreshold(signals.AggregatedForecast, confirmingSignalsForecastTreshhold))
            {
                confirmingSignalsPositive = true;
            }
            if (confirmingSignalsPositive)
            {
                PivotPoints = pivotPoints;
                StrategySignal = CaculateStrategySignal(props);
                if (ForecastExceedsMinimumThreshold(StrategySignal, thresholdForTrade))
                {
                    var stopLoss = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Support2 - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Resistance2);
                    var pricePoint = StrategySignal > 0 ? props.Ask : props.Bid;
                    var riskmanager = new RiskManager(StrategySignal, MaximumRisk, props.AccountBalance, props.PipSize, stopLoss, pricePoint); 
                    var position = new Position()
                    {
                        SymbolName = props.SymbolName,
                        PositionType = StrategySignal > 0 ? PositionType.BUY : PositionType.SELL,
                        EntryPrice = StrategySignal > 0 ? props.Ask : props.Bid,
                        StopLoss = stopLoss,
                        TakeProfit = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Pivot - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Pivot),
                        Volume = riskmanager.LotSize,
                        Created = props.CursorDate,
                        ExpirationDate = new DateTime(props.CursorDate.Year, props.CursorDate.Month, props.CursorDate.Day, 23, 0, 0)
                    };
                    PositionInstructions.Add(new OpenInstruction(position, ValidationService));
                }
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

        public List<IPositionInstruction> GetPositionInstructions()
        {
            return PositionInstructions;
        }
    }
}
