using Application.Business;
using Application.Business.Portfolio;
using Application.Business.Forecasts.CarverTrendFollower;
using Application.RiskControl;
using cAlgo.API;
using Domain.Entities;
using Robots.Common;

namespace Robots
{
    public class CarverTrendFollowerWrapper : IFxProStrategyWrapper
    {
        public List<PositionUpdate> PositionInstructions { get; set; }
        public decimal MinimumOpeningForecast { get; private set; }
        public decimal StopLossMax { get; private set; }
        public decimal TargetVolatility { get; private set; }
        public decimal TrailStopAtPips { get; private set; }
        public decimal TrailStopSizeInPips { get; private set; }
        public double TakeProfitInPips { get; private set; }
        public List<Test_Parameters> TestParameters { get; private set; }

    public CarverTrendFollowerWrapper(decimal currentCapital, Bars bars, Positions positions, string symbolName, string dataSource, double askingPrice, double biddingPrice)//,
            //List<Test_Parameters>? testParameters)
        {
            var barDat = new BarConvert();
            var barData = new List<List<HistoricalData>>()
            {
                barDat.GetHistoData(bars)
            };
        
            //LoadTestParameters(testParameters);
            var cursorDate = bars.OpenTimes.Last();
            //var logger = new Logger(false);
            var carverTrendFollower = new CarverTrendFollowerForecast();
            var forecasts = carverTrendFollower.GetForecasts(barData, cursorDate, askingPrice, biddingPrice);//, testParameters);
            var weightedPositions = new WeightedProposedPositions(forecasts, StopLossMax, 1M, TargetVolatility, barData, currentCapital);
            //foreach (var pos in weightedPositions)
            //    PikUpStix.Trading.Common.FileWriter.WriteToTextFile(new string[] { cursorDate.ToString() + pos.ProposedWeightedPosition.ToString() }, @"C:\Users\Finn\Desktop\text.txt");

            PositionInstructions = new List<PositionUpdate>();

            // Close positions with no weightedPosition
            foreach (var p in positions)
                if (!weightedPositions.Where(x => x.Instrument.InstrumentName == p.SymbolName).Any())
                {
                    var weightedPos = weightedPositions.First(x => x.Instrument.InstrumentName == p.SymbolName);
                    PositionInstructions.Add(new PositionUpdate(p, 0, 0, InstructionType.Close, Convert.ToDouble(weightedPos.ProposedWeightedPosition), Convert.ToDouble(weightedPos.ProposedWeightedPosition)));
                }

            // Modify
            foreach (var wp in weightedPositions)
            {
                foreach (var p in positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName))
                {
                    if (AreSameDirection(wp, p))
                    {
                        PositionInstructions.Add(new PositionUpdate(p, CalculateStopLoss(askingPrice, biddingPrice, wp, p, barDat.Instrument), 200, InstructionType.Modify, GetVolume(wp), Convert.ToDouble(wp.ProposedWeightedPosition)));
                    }
                    else
                    {
                        // Close Position
                        PositionInstructions.Add(new PositionUpdate(p, 0, 0, InstructionType.Close, 0, Convert.ToDouble(wp.ProposedWeightedPosition)));
                        // Open new position in opposite direction
                        if (wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0)
                        {
                            PositionInstructions.Add(new PositionUpdate(GetTradeType(wp), Convert.ToDouble(wp.StopLossInPips), TakeProfitInPips, InstructionType.Open, GetVolume(wp), Convert.ToDouble(wp.ProposedWeightedPosition)));
                        }
                    }
                }
            }

            // Add new positions
            foreach (var wp in weightedPositions)
            {
                if (!positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName).Any())
                {
                    if (wp.ForecastValue.Forecast > MinimumOpeningForecast || wp.ForecastValue.Forecast < MinimumOpeningForecast * -1)
                    {
                        if (wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0)
                            PositionInstructions.Add(new PositionUpdate(GetTradeType(wp), Convert.ToDouble(wp.StopLossInPips), TakeProfitInPips, InstructionType.Open, GetVolume(wp), Convert.ToDouble(wp.ProposedWeightedPosition)));
                    }
                }
            }
        }

        private double CalculateStopLoss(double askingPrice, double biddingPrice, PositionValue wp, Position p, Instrument instrument)
        {
            var trailingStopAt = Convert.ToDouble(instrument.MinimumPriceFluctuation * TrailStopAtPips);
            var trailingStopSize = Convert.ToDouble(instrument.MinimumPriceFluctuation * TrailStopSizeInPips);
            var trailingStop = new TrailingStop(GetTradeType(wp), p.EntryPrice, Convert.ToDouble(wp.StopLossAt), biddingPrice, askingPrice, trailingStopAt, trailingStopSize);
            if (trailingStop.TrailingStopUpdated)
                return trailingStop.TrailingStopAt;
            if (p.StopLoss >= trailingStop.TrailingStopAt && Convert.ToDouble(wp.StopLossAt) < trailingStop.TrailingStopAt)
                return trailingStop.TrailingStopAt;
            return Convert.ToDouble(wp.StopLossAt);
        }

        private void LoadTestParameters(List<Test_Parameters>? testParameters)
        {
            if (testParameters != null)
            {
                PropertyChecker.CheckExists("MaxStopLoss[Double]", testParameters);
                PropertyChecker.CheckExists("TargetVelocity[Double]", testParameters);
                PropertyChecker.CheckExists("MinimumOpeningForecast[Double]", testParameters);
                PropertyChecker.CheckExists("TrailStopAtPips[Double]", testParameters);
                PropertyChecker.CheckExists("TrailStopSizeInPips[Double]", testParameters);
                PropertyChecker.CheckExists("TakeProfitInPips[Double]", testParameters);
                PropertyChecker.CheckExists("ShortScalar[Double]", testParameters);
                PropertyChecker.CheckExists("MediumScalar[Double]", testParameters);
                PropertyChecker.CheckExists("LongScalar[Double]", testParameters);

                foreach (var param in testParameters)
                {
                    if (param.Name.Equals("MaxStopLoss[Double]"))
                        StopLossMax = Convert.ToDecimal(param.Value);
                    if (param.Name.Equals("TargetVelocity[Double]"))
                        TargetVolatility = Convert.ToDecimal(param.Value);
                    if (param.Name.Equals("MinimumOpeningForecast[Double]"))
                        MinimumOpeningForecast = Convert.ToDecimal(param.Value);
                    if (param.Name.Equals("TrailStopAtPips[Double]"))
                        TrailStopAtPips = Convert.ToDecimal(param.Value);
                    if (param.Name.Equals("TrailStopSizeInPips[Double]"))
                        TrailStopSizeInPips = Convert.ToDecimal(param.Value);
                    if (param.Name.Equals("TakeProfitInPips[Double]"))
                        TakeProfitInPips = Convert.ToDouble(param.Value);
                }
            }
            else
            {
                /// TODO : GET RID OF THIS ONCE REFACTORING COMPLETE
                StopLossMax = 50;
                TargetVolatility = Convert.ToDecimal(0.2);
                MinimumOpeningForecast = Convert.ToDecimal(0.2);
                TrailStopAtPips = Convert.ToDecimal(50);
            }
        }

        private double GetVolume(PositionValue wp)
        {
            if (wp.ProposedWeightedPosition < 0)
            {
                // Change proposed weight to a positive number for cTrader (which takes sell volumes in positive with tradetype.sell
                return Convert.ToDouble(wp.ProposedWeightedPosition * -1);
            }
            return Convert.ToDouble(wp.ProposedWeightedPosition);
        }

        private TradeType GetTradeType(PositionValue wp)
        {
            return wp.ProposedWeightedPosition >= 0 ? TradeType.Buy : TradeType.Sell;
        }

        private bool AreSameDirection(PositionValue wp, Position p)
        {
            if (wp.ProposedWeightedPosition > 0 && p.TradeType.Equals(TradeType.Buy))
                return true;
            if (wp.ProposedWeightedPosition < 0 && p.TradeType.Equals(TradeType.Sell))
                return true;
            return false;
        }
    }
}
