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
                        Name = "MaxStopLoss",
                        Value = "2"
                    },
                    new Test_Parameter
                    {
                        Name = "TargetVelocity",
                        Value = "0.25"
                    },
                    new Test_Parameter {
                        Name = "MinimumOpeningForecast",
                        Value = "0.5"
                    },
                    new Test_Parameter
                    {
                        Name = "ShortScalar",
                        Value = "0.4"
                    },
                    new Test_Parameter {
                        Name = "MediumScalar",
                        Value = "0.2"
                    },
                    new Test_Parameter
                    {
                        Name = "LongScalar",
                        Value = "0.4"
                    }
                };
        }
    }
}
