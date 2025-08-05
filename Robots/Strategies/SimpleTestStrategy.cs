using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Business.Positioning;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Robots.Strategies
{
    public class SimpleTestStrategy : BaseStrategy
    {
        public IValidationService GetValidationService()
        {
            return ValidationService;
        }

        public void SetValidationService(IValidationService value)
        {
            ValidationService = value;
        }

        public List<string> RequiredParameters = new List<string>
            {
                "RiskPerTrade[Double]",
                "Periods[Int]",
                "VolumeWeight[Double]",
                "StopLossAmount[Double]"
            };

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            PositionInstructions.Clear();
            foreach (var marketInfo in marketInfos)
            {
                if (!marketInfo.Positions.Where(x => x.Status == PositionStatus.OPEN).Any())
                {
                    Position position = PositionCreator.CreatePosition(PositionType.BUY, 1, 0.02, 0.01, 0.02, marketInfo, null);
                    PositionInstructions.Add(new OpenInstruction(position, GetValidationService()));
                    LogMessages.Add($"Buy signal for {marketInfo.SymbolName} at price {marketInfo.Ask}");
                }
            }
            return PositionInstructions;
        }

        public void LoadDefaultParameters(Dictionary<string, string> parameters)
        {
            TestParameters.Clear();

            if (parameters == null)
            {
                double defaultRiskPerTrade = 0.01;
                int defaultPeriod = 20;
                double defaultVolumeWeight = 0.5; // Example: 50% volume weight
                double defaultStopLossAmount = 0.02;

                Console.WriteLine("Input parameters dictionary is null. Using hardcoded default values.");

                TestParameters.Add(new Test_Parameter() { Name = "RiskPerTrade[Double]", Value = defaultRiskPerTrade.ToString() });
                TestParameters.Add(new Test_Parameter() { Name = "Periods[Int]", Value = defaultPeriod.ToString() });
                // Calculate PriceWeight based on VolumeWeight, assuming the same logic as original.
                TestParameters.Add(new Test_Parameter() { Name = "PriceWeight[Double]", Value = (1 - defaultVolumeWeight).ToString() });
                TestParameters.Add(new Test_Parameter() { Name = "VolumeWeight[Double]", Value = defaultVolumeWeight.ToString() });
                TestParameters.Add(new Test_Parameter() { Name = "StopLossAmount[Double]", Value = defaultStopLossAmount.ToString() });
            }
            else
            {
                if(!ConfirmParametersValid(parameters.Select(p => new Test_Parameter { Name = p.Key, Value = p.Value }).ToList()))
                {
                    Console.WriteLine("Invalid parameters provided. Using default values.");
                    LoadDefaultParameters(null); // Recurse with null to use defaults
                    return;
                }
                // Iterate through the provided dictionary's key-value pairs.
                foreach (var paramEntry in parameters)
                {
                    TestParameters.Add(new Test_Parameter() { Name = paramEntry.Key, Value = paramEntry.Value });
                }
                Console.WriteLine($"Parameters loaded from input dictionary. Total entries: {TestParameters.Count}");
            }

        }
        public bool ConfirmParametersValid(List<Test_Parameter>? testParameters)
        {
            if (testParameters == null || !testParameters.Any())
            {
                Console.WriteLine("No test parameters provided. Using default values.");
                return false; 
            }
            // Check if all required parameters are present
            
            foreach (var param in RequiredParameters)
            {
                if (!testParameters.Any(p => p.Name == param))
                {
                    Console.WriteLine($"Missing required parameter: {param}");
                    return false; // Required parameter is missing
                }
            }
            return true; // All required parameters are present
        }
    }
}
