using Application.Business;
using Application.Business.Portfolio;
using Application.Business.Forecasts.CarverTrendFollower;
using Application.RiskControl;
using Domain.Entities;
using Robots.Common;
using Application.BackTest;
using Domain.Enums;
using Robots.Interfaces;
using Application.Business.Market;

namespace Robots.Strategies.Trend
{
    public class CarverTrendStrategy : IStrategy
    {
        public List<PositionUpdate> PositionInstructions { get; set; } = new List<PositionUpdate>();
        public List<string> LogMessages { get; set; } = new List<string>();
        public double MinimumOpeningForecast { get; private set; }
        public double StopLossMax { get; private set; }
        public double TargetVolatility { get; private set; }
        public double TrailStopAtPips { get; private set; }
        //public double TrailStopSizeInPips { get; private set; }
        //public double TakeProfitInPips { get; private set; }
        public List<Test_Parameter> TestParameters { get; private set; }

        public CarverTrendStrategy(List<IMarketInfo> marketInfos, List<Test_Parameter>? testParameters)
        {
            LoadTestParameters(testParameters);
            var forecasts = CarverTrendFollowerForecasts.GetForecasts(marketInfos, new Logger(false), testParameters);
            var weightedPositions = new WeightedProposedPositions(forecasts, StopLossMax, 1, TargetVolatility, marketInfos);

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
                if (NoCurrentPositionInMarket(market, wp) && MiniumOpeningForecastMet(wp) && ForecastNotZero(wp))
                {
                    var position = new Position();
                    position.SymbolName = wp.Instrument.InstrumentName;
                    position.StopLoss = Convert.ToDouble(wp.StopLossInPips);
                    //position.TakeProfit = TakeProfitInPips;
                    position.Volume = GetVolume(wp);
                    position.TradeType = GetTradeType(wp);
                    PositionInstructions.Add(new PositionUpdate(position, InstructionType.Open, Convert.ToDouble(wp.ProposedWeightedPosition)));
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
                        p.StopLoss = wp.StopLossInPips;// CalculateStopLoss(market.Ask, market.Bid, wp, p, wp.Instrument);
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
                            position.StopLoss = wp.StopLossInPips;
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
        private static bool ForecastNotZero(PositionValue wp)
        {
            return wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0;
        }

        private static bool NoCurrentPositionInMarket(IMarketInfo market, PositionValue wp)
        {
            return !market.Positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName).Any();
        }

        private bool MiniumOpeningForecastMet(PositionValue wp)
        {
            return wp.ForecastValue.Forecast > MinimumOpeningForecast || wp.ForecastValue.Forecast < MinimumOpeningForecast * -1;
        }
        private void LoadTestParameters(List<Test_Parameter>? testParameters)
        {
            if (testParameters != null)
            {
                PropertyChecker.CheckExists("MaxStopLoss[Double]", testParameters);
                PropertyChecker.CheckExists("TargetVelocity[Double]", testParameters);
                PropertyChecker.CheckExists("MinimumOpeningForecast[Double]", testParameters);
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
                }
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
