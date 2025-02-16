using Application.Business;
using Application.Business.Portfolio;
using Application.Business.Forecasts.CarverTrendFollower;
using Application.RiskControl;
using Domain.Entities;
using Robots.Common;
using Application.BackTest;
using Domain.Enums;
using Robots.Interfaces;

namespace Robots.Strategies.CarverTrendFollower
{
    public class CarverTrendFollowerStrategy : IStrategy
    {
        public List<PositionUpdate> PositionInstructions { get; set; }
        public List<string> LogMessages { get; set; } = new List<string>();
        public double MinimumOpeningForecast { get; private set; }
        public double StopLossMax { get; private set; }
        public double TargetVolatility { get; private set; }
        public double TrailStopAtPips { get; private set; }
        public double TrailStopSizeInPips { get; private set; }
        public double TakeProfitInPips { get; private set; }
        public List<Test_Parameter> TestParameters { get; private set; }

        public CarverTrendFollowerStrategy(List<IMarketInfo> marketInfos, List<Test_Parameter>? testParameters)
        {
            LoadTestParameters(testParameters);

            var logger = new Logger(false);
            var carverTrendFollower = new CarverTrendFollowerForecast();
            var forecasts = carverTrendFollower.GetForecasts(marketInfos,logger, testParameters);
            var weightedPositions = new WeightedProposedPositions(forecasts, StopLossMax, 1, TargetVolatility, marketInfos);
            //foreach (var pos in weightedPositions)
            //    FileWriter.WriteToTextFile(new string[] { cursorDate.ToString() + pos.ProposedWeightedPosition.ToString() }, @"C:\Users\Finn\Desktop\text.txt");

            PositionInstructions = new List<PositionUpdate>();

            foreach (var market in marketInfos)
            {
                ClosePositionsWithNoForecasts(weightedPositions, market);
                ModifyExistingPositions(weightedPositions, market);
                CreateNewPositions(weightedPositions, market);
            }
        }

        private void CreateNewPositions(WeightedProposedPositions weightedPositions, IMarketInfo market)
        {
            // Add new positions
            foreach (var wp in weightedPositions.Where(x => x.Instrument.InstrumentName == market.SymbolName))
            {
                if (!market.Positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName).Any())
                {
                    if (wp.ForecastValue.Forecast > MinimumOpeningForecast || wp.ForecastValue.Forecast < MinimumOpeningForecast * -1)
                    {
                        if (wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0)
                        {
                            var position = new Position();
                            position.SymbolName = wp.Instrument.InstrumentName;
                            position.StopLoss = Convert.ToDouble(wp.StopLossInPips);
                            position.TakeProfit = TakeProfitInPips;
                            position.Volume = GetVolume(wp);
                            position.TradeType = GetTradeType(wp);
                            PositionInstructions.Add(new PositionUpdate(position, InstructionType.Open, Convert.ToDouble(wp.ProposedWeightedPosition)));
                        }
                    }
                }
            }
        }

        private void ModifyExistingPositions(WeightedProposedPositions weightedPositions, IMarketInfo market)
        {
            //MODIFY EXISTING POSITIONS
            foreach (var wp in weightedPositions.Where(x => x.Instrument.InstrumentName == market.SymbolName))
            {
                foreach (var p in market.Positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName))
                {
                    if (AreSameDirection(wp, p))
                    {
                        p.StopLoss = CalculateStopLoss(market.Ask, market.Bid, wp, p, wp.Instrument);
                        p.TakeProfit = 200;
                        p.Volume = GetVolume(wp);
                        PositionInstructions.Add(
                            new PositionUpdate(p, InstructionType.Modify, Convert.ToDouble(wp.ProposedWeightedPosition)));
                    }
                    else
                    {
                        // Close Position
                        PositionInstructions.Add(new PositionUpdate(p, InstructionType.Close, Convert.ToDouble(wp.ProposedWeightedPosition)));
                        // Open new position in opposite direction
                        if (wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0)
                        {
                            var position = new Position();
                            position.SymbolName = wp.Instrument.InstrumentName;
                            position.StopLoss = Convert.ToDouble(wp.StopLossInPips);
                            position.TakeProfit = TakeProfitInPips;
                            p.TradeType = p.TradeType == TradeType.Buy ? TradeType.Sell : TradeType.Buy;
                            position.Volume = GetVolume(wp);
                            PositionInstructions.Add(new PositionUpdate(position, InstructionType.Open, Convert.ToDouble(wp.ProposedWeightedPosition)));
                        }
                    }
                }
            }
        }

        private void ClosePositionsWithNoForecasts(WeightedProposedPositions weightedPositions, IMarketInfo market)
        {
            //CLOSE CURRENT POSITION IF THERE IS NO WEIGHTED POSITION CALCULATED FOR IT
            foreach (var p in market.Positions)
                if (!weightedPositions.Where(x => x.Instrument.InstrumentName == p.SymbolName).Any())
                {
                    var weightedPos = weightedPositions.First(x => x.Instrument.InstrumentName == p.SymbolName);
                    PositionInstructions.Add(
                        new PositionUpdate(p, InstructionType.Close,
                        Convert.ToDouble(weightedPos.ProposedWeightedPosition)));
                }
        }

        private double CalculateStopLoss(double askingPrice, double biddingPrice, PositionValue wp, Position p, Instrument instrument)
        {
            var trailingStopAt = Convert.ToDouble(instrument.MinimumPriceFluctuation * TrailStopAtPips);
            var trailingStopSize = Convert.ToDouble(instrument.MinimumPriceFluctuation * TrailStopSizeInPips);
            var trailingStop = new TrailingStop(GetTradeType(wp), Convert.ToDouble(p.EntryPrice), Convert.ToDouble(wp.StopLossAt), biddingPrice, askingPrice, trailingStopAt, trailingStopSize);
            if (trailingStop.TrailingStopUpdated)
                return trailingStop.TrailingStopAt;
            if (p.StopLoss >= trailingStop.TrailingStopAt && Convert.ToDouble(wp.StopLossAt) < trailingStop.TrailingStopAt)
                return trailingStop.TrailingStopAt;
            return Convert.ToDouble(wp.StopLossAt);
        }

        private void LoadTestParameters(List<Test_Parameter>? testParameters)
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
                        StopLossMax = Convert.ToDouble(param.Value);
                    if (param.Name.Equals("TargetVelocity[Double]"))
                        TargetVolatility = Convert.ToDouble(param.Value);
                    if (param.Name.Equals("MinimumOpeningForecast[Double]"))
                        MinimumOpeningForecast = Convert.ToDouble(param.Value);
                    if (param.Name.Equals("TrailStopAtPips[Double]"))
                        TrailStopAtPips = Convert.ToDouble(param.Value);
                    if (param.Name.Equals("TrailStopSizeInPips[Double]"))
                        TrailStopSizeInPips = Convert.ToDouble(param.Value);
                    if (param.Name.Equals("TakeProfitInPips[Double]"))
                        TakeProfitInPips = Convert.ToDouble(param.Value);
                }
            }
            else
            {
                /// TODO : GET RID OF THIS ONCE REFACTORING COMPLETE
                StopLossMax = 50;
                TargetVolatility = Convert.ToDouble(0.2);
                MinimumOpeningForecast = Convert.ToDouble(0.2);
                TrailStopAtPips = Convert.ToDouble(50);
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
            if (wp.ProposedWeightedPosition > 0 && p.TradeType == TradeType.Buy)
                return true;
            if (wp.ProposedWeightedPosition < 0 && p.TradeType == TradeType.Sell)
                return true;
            return false;
        }
    }
}
