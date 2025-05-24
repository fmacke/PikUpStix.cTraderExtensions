using Application.Business.Calculations;
using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Business.Positioning;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System.Diagnostics;

namespace Robots.Strategies
{
    public class VolumePriceAnalysis : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<IPositionInstruction> PositionInstructions { get; private set; } = new List<IPositionInstruction>();
        public IValidationService ValidationService { get; set; } = new ValidationService();
        public List<Test_Parameter> TestParameters { get; set; }

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            var dataAmount = 21; // Number of bars to consider for volume analysis
            PositionInstructions.Clear();

            foreach (var marketInfo in marketInfos)
            {
                if (marketInfo?.Bars == null || marketInfo.Bars.Count < dataAmount)
                {
                    LogMessages.Add($"Insufficient data for {marketInfo?.SymbolName ?? "Unknown Symbol"}");
                    continue;
                }

                // Calculate historical volume stats
                var recentMarketData = marketInfo.Bars.Select(b => b.Volume).TakeLast(dataAmount).ToArray();
                recentMarketData = recentMarketData.Take(recentMarketData.Length - 1).ToArray();  // remove current bar volume
                double meanVolume = recentMarketData.Average();
                double volumeStdDev = new StandardDeviation(recentMarketData).Calculate();

                // Calculate historical price stats
                var averagePrices = marketInfo.Bars.Select(b => (b.OpenPrice + b.HighPrice + b.LowPrice) / 3).TakeLast(dataAmount).ToArray();
                averagePrices = averagePrices.Take(averagePrices.Length - 1).ToArray(); // remove current bar price
                double meanPrice = averagePrices.Average();
                double priceStdDev = new StandardDeviation(averagePrices).Calculate();

                // Current market data
                var volume = marketInfo.CurrentBar.Volume;
                var price = marketInfo.Ask;

                // **Compute Forecast (-1 to 1)**
                double normalizedPriceChange = (price - meanPrice) / priceStdDev;
                double normalizedVolumeStrength = (volume - meanVolume) / volumeStdDev;

                // Combine and weight price vs. volume impact
                double forecast = Math.Clamp((0.7 * normalizedPriceChange) + (0.3 * normalizedVolumeStrength), -1, 1);
                Debug.Print(forecast.ToString("F2"));

                //// **Close Positions If Forecast Turns Against Them**
                foreach (var position in marketInfo.Positions.Where(p => p.Status == PositionStatus.OPEN))
                {
                    if ((position.PositionType == PositionType.BUY && forecast < 0) ||
                        (position.PositionType == PositionType.SELL && forecast > 0))
                    {
                        position.Comment = $"Closing position due to forecast reversal (Forecast: {forecast:F2})";
                        PositionInstructions.Add(new CloseInstruction(position, marketInfo.Ask, marketInfo.CursorDate, ValidationService));
                        LogMessages.Add($"Closing {position.PositionType} position for {marketInfo.SymbolName} (Forecast reversed to {forecast:F2})");
                    }
                }

                //// **Open New Positions Based on Forecast**
                var maxRiskPercentage = 0.05; // 5% of current capital
                var stopLossAmount = 0.1; // 10 pips stop loss
                if (forecast > 0.3)
                {
                    Position position = PositionCreator.CreatePosition(PositionType.BUY, forecast, maxRiskPercentage, stopLossAmount, null, marketInfo, null);
                    PositionInstructions.Add(new OpenInstruction(position, ValidationService));
                    LogMessages.Add($"Opening Buy position for {marketInfo.SymbolName} (Forecast: {forecast:F2}) at price {price}");
                }
                else if (forecast < -0.3)
                {
                    Position position = PositionCreator.CreatePosition(PositionType.SELL, forecast, maxRiskPercentage, stopLossAmount, null, marketInfo, null);
                    PositionInstructions.Add(new OpenInstruction(position, ValidationService));
                    LogMessages.Add($"Opening Sell position for {marketInfo.SymbolName} (Forecast: {forecast:F2}) at price {price}");
                }
                else
                {
                    LogMessages.Add($"Hold signal for {marketInfo.SymbolName} (Forecast: {forecast:F2}) at price {price}");
                }
            }

            return PositionInstructions;
        }

        public double CalculateAverageElements(double[] values, int numberOfPeriods)
        {
            if (values == null || values.Length == 0 || numberOfPeriods <= 0)
            {
                throw new ArgumentException("Invalid input: array must not be null or empty, and X must be greater than 0.");
            }

            return values.TakeLast(numberOfPeriods).Average();
        }
    }
}