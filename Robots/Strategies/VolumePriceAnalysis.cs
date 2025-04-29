using Application.Business.Calculations;
using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

public class VolumePriceAnalysis : IStrategy
{
    public List<string> LogMessages { get; set; } = new List<string>();
    public List<Test_Parameter> TestParameters { get; set; } = new List<Test_Parameter>();
    private List<IPositionInstruction> _positionInstructions = new List<IPositionInstruction>();
    public IValidationService ValidationService { get; set; } = new ValidationService();

    public List<IPositionInstruction> Run(List<IMarketInfo> marketInfos)
    {
        _positionInstructions.Clear();
        foreach (var marketInfo in marketInfos)
        {
            if(marketInfo == null || marketInfo.Bars == null || marketInfo.Bars.Count < 20)
            {
                LogMessages.Add($"Insufficient data for {marketInfo.SymbolName}");
                break;
            }
            double meanVolume = CalculateAverageElements(marketInfo.Bars.Select(b => b.Volume).ToArray(), 20); // Calculate the mean volume over the last 20 bars            
            double standardDeviation = new StandardDeviation(
                GetLastResults(marketInfo.Bars.Select(b => b.Volume).ToArray(), 20))
                .Calculate(); // Calculate the standard deviation of volume
            double volumeThreshold = meanVolume + (2 * standardDeviation); // Define a threshold for significant volume change

            var volume = marketInfo.LastBar.Volume;
            var price = marketInfo.Ask;
            var previousPrice = marketInfo.LastBar.ClosePrice;

            if (volume > volumeThreshold && price > previousPrice)
            {
                // Strong buying interest, potential uptrend continuation
                var stopLoss = marketInfo.Ask - (marketInfo.Ask - 0.001); // Example stop loss calculation
                var stopLossPrice = marketInfo.Ask - stopLoss;
                var takeProfit = marketInfo.Ask - (marketInfo.Ask - 0.001); // Example stop loss calculation                    
                var positionSize =  new PositionSize(1, 0.04, marketInfo.CurrentCapital, marketInfo.PipSize, marketInfo.LotSize, stopLossPrice, marketInfo.Ask).Calculate();
                var position = new Position()
                {
                    SymbolName = marketInfo.SymbolName,
                    PositionType = PositionType.BUY,
                    EntryPrice = marketInfo.Ask,
                    StopLoss = stopLoss,
                    TakeProfit = stopLoss,
                    Volume = positionSize,
                    Created = marketInfo.CursorDate,
                    ExpirationDate = new DateTime(marketInfo.CursorDate.Year, marketInfo.CursorDate.Month, marketInfo.CursorDate.Day, 23, 59, 0)
                };
                _positionInstructions.Add(new OpenInstruction(position, ValidationService));
                LogMessages.Add($"Buy signal for {marketInfo.SymbolName} at price {price}");
            }
            else if (volume > volumeThreshold && price < previousPrice)
            {
                // Strong selling pressure, potential downtrend continuation
                var stopLoss = marketInfo.Ask - (marketInfo.Ask * 0.001); // Example stop loss calculation
                var takeProfit = marketInfo.Ask + (marketInfo.Ask * 0.001); // Example stop loss calculation                    
                var positionSize = new PositionSize(1, 0.02, marketInfo.CurrentCapital, marketInfo.PipSize, marketInfo.LotSize, stopLoss, marketInfo.Ask).Calculate();
                var position = new Position()
                {
                    SymbolName = marketInfo.SymbolName,
                    PositionType = PositionType.SELL,
                    EntryPrice = marketInfo.Ask,
                    StopLoss = stopLoss,
                    //TakeProfit = takeProfit,
                    Volume = positionSize,
                    Created = marketInfo.CursorDate,
                    ExpirationDate = new DateTime(marketInfo.CursorDate.Year, marketInfo.CursorDate.Month, marketInfo.CursorDate.Day, 23, 59, 0)
                };
                _positionInstructions.Add(new OpenInstruction(position, ValidationService));
                LogMessages.Add($"Sell signal for {marketInfo.SymbolName} at price {price}");
            }
            else if (volume < volumeThreshold && price > previousPrice)
            {
                // Weak buying interest, potential trend reversal
                foreach(var position in marketInfo.Positions.Where(p => p.Status == PositionStatus.OPEN && p.PositionType == PositionType.BUY))
                {
                    _positionInstructions.Add(new CloseInstruction(position, marketInfo.Ask, marketInfo.CursorDate, ValidationService));
                }
                LogMessages.Add($"Hold signal for {marketInfo.SymbolName} at price {price}");
            }
            else if (volume < volumeThreshold && price < previousPrice)
            {
                // Weak selling pressure, potential trend reversal
                foreach (var position in marketInfo.Positions.Where(p => p.Status == PositionStatus.OPEN && p.PositionType == PositionType.SELL))
                {
                    _positionInstructions.Add(new CloseInstruction(position, marketInfo.Ask, marketInfo.CursorDate, ValidationService));
                }
                LogMessages.Add($"Hold signal for {marketInfo.SymbolName} at price {price}");
            }
        }
        return _positionInstructions;
    }

    private double[] GetLastResults(double[] values, int numberOfPeriods)
    {
        if (numberOfPeriods > values.Length)
            numberOfPeriods = values.Length;
        return values.Skip(values.Length - (1+numberOfPeriods)).ToArray();  // Get the last N periods but not current period (i.e. last value)
    }

    public double CalculateAverageElements(double[] values, int numberOfPeriods)
    {
        if (values == null || values.Length == 0 || numberOfPeriods <= 0)
        {
            throw new ArgumentException("Invalid input: array must not be null or empty, and X must be greater than 0.");
        }

        int startIndex = Math.Max(0, values.Length - numberOfPeriods);
        double sum = 0;
        int count = 0;

        for (int i = startIndex; i < values.Length; i++)
        {
            sum += values[i];
            count++;
        }

        return count > 0 ? sum / count : 0;
    }

}
