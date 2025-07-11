﻿using Application.Business;
using Application.Business.Portfolio;
using Application.Business.Forecasts.CarverTrendFollower;
using Domain.Entities;
using Domain.Enums;
using Application.Business.Market;
using Application.Business.Positioning.Validation;
using Application.Interfaces;
using Application.Business.Positioning.Instructions;

namespace Robots.Strategies.CarverTrendFollower
{
    public class CarverTrendFollowerStrategy : IStrategy
    {
        public List<IMarketInfo> MarketInfos { get; set; }
        private List<IPositionInstruction> _positionInstructions = new List<IPositionInstruction>();
        public List<string> LogMessages { get; set; } = new List<string>();
        public double MinimumOpeningForecast { get; private set; }
        public double StopLossMax { get; private set; }
        public double TargetVolatility { get; private set; }
        public double TrailStopAtPips { get; private set; }
        public List<Test_Parameter> TestParameters { get; set; }
        public IValidationService ValidationService { get; set; } = new ValidationService();

        public CarverTrendFollowerStrategy(List<Test_Parameter>? testParameters)
        {
            ConfirmParametersValid(testParameters);
            TestParameters = testParameters;            
        }

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            _positionInstructions = new List<IPositionInstruction>();
            MarketInfos = marketInfos;
            var forecasts = CarverTrendFollowerForecasts.GetForecasts(MarketInfos, TestParameters);
            var weightedPositions = new WeightedProposedPositions(forecasts, StopLossMax, 1, TargetVolatility, MarketInfos);

            foreach (var market in MarketInfos)
            {
                ClosePositionsWithNoForecasts(weightedPositions, market);
                ModifyExistingPositions(weightedPositions, market);
                CreateNewPositions(weightedPositions, market);
            }
            return _positionInstructions;
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
                    position.EntryPrice = market.Ask;
                    position.Volume = GetVolume(wp);
                    position.PositionType = GetTradeType(wp);
                    position.Created  = market.CursorDate;
                    _positionInstructions.Add(new OpenInstruction(position, ValidationService));
                }
            }
        }       

        private void ModifyExistingPositions(WeightedProposedPositions weightedPositions, IMarketInfo market)
        {
            //MODIFY EXISTING POSITIONS
            foreach (var wp in weightedPositions.Where(x => x.Instrument.InstrumentName == market.SymbolName))
            {
                foreach (var p in market.Positions.Where(x => x.SymbolName == wp.Instrument.InstrumentName  && x.Status == PositionStatus.OPEN))
                {
                    if (AreSameDirection(wp, p))
                    {
                        p.StopLoss = wp.StopLossInPips;// CalculateStopLoss(market.Ask, market.Bid, wp, p, wp.Instrument);
                        p.Volume = GetVolume(wp);
                        _positionInstructions.Add(
                            new ModifyInstruction(p, wp.StopLossAt, 0, ValidationService));
                    }
                    else
                    {
                        // Close Position
                        _positionInstructions.Add(new CloseInstruction(p, market.Bid, market.CursorDate, ValidationService));
                        // Open new position in opposite direction
                        if (wp.ProposedWeightedPosition > 0 || wp.ProposedWeightedPosition < 0)
                        {
                            var position = new Position();
                            position.SymbolName = wp.Instrument.InstrumentName;
                            position.StopLoss = wp.StopLossInPips;
                            p.PositionType = p.PositionType == PositionType.BUY ? PositionType.SELL : PositionType.BUY;
                            position.Volume = GetVolume(wp);
                            position.EntryPrice = market.Ask;
                            _positionInstructions.Add(new OpenInstruction(position, ValidationService));
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
                    _positionInstructions.Add(
                        new CloseInstruction(p, market.Bid, market.CursorDate, ValidationService));
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
        private void ConfirmParametersValid(List<Test_Parameter>? testParameters)
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

        private PositionType GetTradeType(PositionValue wp)
        {
            return wp.ProposedWeightedPosition >= 0 ? PositionType.BUY : PositionType.SELL;
        }

        private bool AreSameDirection(PositionValue wp, Position p)
        {
            if (wp.ProposedWeightedPosition > 0 && p.PositionType == PositionType.BUY)
                return true;
            if (wp.ProposedWeightedPosition < 0 && p.PositionType == PositionType.SELL)
                return true;
            return false;
        }

        
    }
}
