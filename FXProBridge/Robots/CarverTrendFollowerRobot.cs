﻿using Application.Business.Extensions;
using Application.Business.Indicator.Signal;
using Application.Business.Market;
using cAlgo.API;
using cAlgo.API.Internals;
using Domain.Enums;
using FXProBridge.Common;
using FXProBridge.DataConversions;
using Robots.Strategies.CarverTrendFollower;

namespace FXProBridge.Robots
{
    public abstract class CarverTrendFollowerRobot : PositionManager
    {
        [Parameter("Max Stop Loss", Group = "Risk Management", DefaultValue = 2, MinValue = 1, Step = 1)]
        public double MaxStopLoss { get; set; }
        [Parameter("Target Velocity", Group = "Performance", DefaultValue = 0.25, MinValue = 0.01, Step = 0.01)]
        public double TargetVelocity { get; set; }
        [Parameter("Minimum Opening Forecast", Group = "Forecasting", DefaultValue = 0.0, MinValue = 0.0, Step = 0.1)]
        public double MinimumOpeningForecast { get; set; }
        [Parameter("Short Scalar", Group = "Scalars", DefaultValue = 0.4, MinValue = 0.1, Step = 0.1)]
        public double ShortScalar { get; set; }
        [Parameter("Medium Scalar", Group = "Scalars", DefaultValue = 0.2, MinValue = 0.1, Step = 0.1)]
        public double MediumScalar { get; set; }
        [Parameter("Long Scalar", Group = "Scalars", DefaultValue = 0.4, MinValue = 0.1, Step = 0.1)]
        public double LongScalar { get; set; }

        protected override void OnStart()
        {
            TestParams = ParametersToDictionary.GetRobotProperties((CarverTrendFollowerRobot)this);
            base.OnStart();
        }

        protected override void OnBar()
        {
            var changeInstructions = new CarverTrendFollowerStrategy(ResultsCapture.TestParams);
            var exchangeRate = 1;
            ManagePositions(changeInstructions.CalculateChanges(
                new List<IMarketInfo> {
                    new MarketInfo(Bars.OpenTimes.LastValue,
                    Symbol.Bid,
                    Symbol.Ask,
                    PositionConvert.ConvertPosition(Positions),
                    BarConvert.ConvertBars(Bars),
                    SymbolName,
                    SymbolName,
                    Account.Equity,
                    Symbol.PipSize, 
                    Symbol.LotSize,
                    exchangeRate, 
                    new ConfirmingSignals(new List<ISignal>()), 
                    Domain.Enums.TimeFrame.D1)}));
            base.OnBar();
        }
    }
}

















