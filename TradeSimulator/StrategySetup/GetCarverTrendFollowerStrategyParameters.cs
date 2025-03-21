using Domain.Entities;

namespace TradeSimulator.StrategySetup
{
    internal class GetCarverTrendFollowerStrategyParameters : IGetStrategyParameters
    {
        public  List<Test_Parameter> Run()
        {
            return new List<Test_Parameter>
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
    }
}
