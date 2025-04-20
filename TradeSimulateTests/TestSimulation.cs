using Domain.Entities;
using Robots.Strategies.CarverTrendFollower;
using Application.Business.Market;
using Application.Business.Indicator.Signal;
using TradeSimulator.Simulate;

namespace TradeSimulateTests
{
    [TestClass]
    public sealed class TestSimulation
    {
        IMarketInfo MarketInfo;
        List<Test_Parameter> TestParameters;    

        [TestInitialize]
        public void TestInitialize()
        {
            LoadBars();
            LoadTestParameters();
        }

        [TestMethod]
        public void RunSimulation()
        {
            var strategy = new CarverTrendFollowerStrategy(TestParameters);
            var tradeSimulator = new TradeSimulate(MarketInfo, strategy, 10000, false);
            tradeSimulator.Run();
            Assert.IsNotNull(tradeSimulator.CurrentMarketInfo.CurrentBar.HighPrice);
        }

        private void LoadTestParameters()
        {
            TestParameters = new List<Test_Parameter>
                {
                    new Test_Parameter {
                        Name = "MaxStopLoss[Double]",
                        Value = "2"
                    },
                    new Test_Parameter
                    {
                        Name = "TargetVelocity[Double]",
                        Value = "0.25"
                    },
                    new Test_Parameter {
                        Name = "MinimumOpeningForecast[Double]",
                        Value = "0.5"
                    },
                    new Test_Parameter
                    {
                        Name = "ShortScalar[Double]",
                        Value = "0.4"
                    },
                    new Test_Parameter {
                        Name = "MediumScalar[Double]",
                        Value = "0.2"
                    },
                    new Test_Parameter
                    {
                        Name = "LongScalar[Double]",
                        Value = "0.4"
                    }
                };
        }

        private void LoadBars()
        {
            DateTime startDate = new DateTime(2000, 1, 1);
            MarketInfo = new MarketInfo(
                startDate,
                0,
                0,
                new List<Position>(),
                new List<HistoricalData>(),
                "EURUSD",
                "USD",
                10000,
                100000,
                1,
                new ConfirmingSignals(new List<ISignal>()),
                Domain.Enums.TimeFrame.H1
            );
            var data = new List<HistoricalData>();
            int expander = 1;
            int direction = 1;
            int subCounter = 6;
            for (int x = 0; x < 200; x++)
            {
                MarketInfo.Bars.Add(new HistoricalData()
                {
                    Date = startDate.AddDays(x),
                    OpenPrice = subCounter,
                    ClosePrice = subCounter + expander,
                    HighPrice = subCounter + (2 * expander),
                    LowPrice = subCounter - (2 * expander),
                });
                if (subCounter % 5 == 0 || (subCounter == 1 && direction == -1))
                {
                    if (direction > 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }
                subCounter += direction;
                //Debug.WriteLine($"Date: {Bars[x].Date} Open: {Bars[x].OpenPrice} Close: {Bars[x].ClosePrice} High: {Bars[x].HighPrice} Low: {Bars[x].LowPrice}");
            }
        }

    }
}
