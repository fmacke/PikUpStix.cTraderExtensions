using Domain.Entities;
using Application.Business.Indicator;
using Application.Business.Market;
using Application.Business.Risk;
using Domain.Enums;
using Application.Business.Positioning.Validation;
using Application.Interfaces;
using Application.Business.Positioning.Instructions;

namespace Robots.Strategies.PivotPointBounce
{
    public class PivotPointConfirmStrategy : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public double StrategySignal { get; private set; }
        public PivotPoints PivotPoints { get; private set; }
        public  double MaximumRisk = 0.02;
        public double thresholdForTrade = 0.5;
        public double confirmingSignalsForecastTreshhold = 0.5;
        public IValidationService ValidationService { get; } = new ValidationService();
        public List<Test_Parameter> TestParameters{ get; }

        private List<IPositionInstruction> _positionInstructions = new List<IPositionInstruction>();

        public PivotPointConfirmStrategy(List<Test_Parameter> test_Parameters)
        {
            TestParameters = test_Parameters;
        }
        public List<IPositionInstruction> Run(List<IMarketInfo> marketInfos)
        {           
            foreach (var marketInfo in marketInfos)
            {
                var confirmingSignalsPositive = false;
                if (marketInfo.Signals.Count == 0)
                    confirmingSignalsPositive = true; // no confirming signal filter
                if (marketInfo.Signals.Count > 0 && ForecastExceedsMinimumThreshold(marketInfo.Signals.AggregatedForecast, confirmingSignalsForecastTreshhold))
                    CalculateNewInstructions(marketInfo);
            }
            return _positionInstructions;
        }
        private void CalculateNewInstructions(IMarketInfo marketInfo)
        {
            PivotPoints = marketInfo.Indicators.OfType<PivotPoints>().FirstOrDefault();
            if (PivotPoints != null)
            {
                StrategySignal = CaculateStrategySignal(marketInfo);
                if (ForecastExceedsMinimumThreshold(StrategySignal, thresholdForTrade))
                {
                    var stopLoss = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Support2 - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Resistance2);
                    var pricePoint = StrategySignal > 0 ? marketInfo.Ask : marketInfo.Bid;
                    var riskmanager = new RiskManager(StrategySignal, MaximumRisk, marketInfo.AccountBalance, marketInfo.ContractUnit, stopLoss, pricePoint);
                    var position = new Position()
                    {
                        SymbolName = marketInfo.SymbolName,
                        PositionType = StrategySignal > 0 ? PositionType.BUY : PositionType.SELL,
                        EntryPrice = StrategySignal > 0 ? marketInfo.Ask : marketInfo.Bid,
                        StopLoss = stopLoss,
                        TakeProfit = StrategySignal > 0 ?
                            CalculatePips(PivotPoints.Pivot - PivotPoints.Support1) :
                            CalculatePips(PivotPoints.Resistance1 - PivotPoints.Pivot),
                        Volume = riskmanager.LotSize,
                        Created = marketInfo.CursorDate,
                        ExpirationDate = new DateTime(marketInfo.CursorDate.Year, marketInfo.CursorDate.Month, marketInfo.CursorDate.Day, 23, 0, 0)
                    };
                    _positionInstructions.Add(new OpenInstruction(position, ValidationService));
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
    }
}
