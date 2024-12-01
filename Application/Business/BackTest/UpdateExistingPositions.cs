namespace PikUpStix.Trading.Forecast
{
    //public class UpdateExistingPositions
    //{
    //    private readonly TradingSystemParams Parameters;
    //    private readonly int TestId;
    //    private decimal CurrentMargin;
    //    private Positions ExistingPositions;
    //    private List<List<HistoricalData>> HistoricalDataSets;

    //    private CreatePositions _createPositions;

    //    public UpdateExistingPositions(TradingSystemParams paramsa, int testId, Positions existingPositions, decimal currentMargin)
    //    {
    //        Parameters = paramsa;
    //        TestId = testId;
    //        ExistingPositions = existingPositions;
    //        CurrentMargin = currentMargin;
    //    }

    //    public Positions AdjustExistingPositions(DateTime cursorDate, WeightedProposedPositions weightedPositions,
    //        List<List<HistoricalData>> historicalDataSets)
    //    {
    //        HistoricalDataSets = historicalDataSets;
    //        foreach (PositionValue weightedProposedPosition in weightedPositions.Where(weightedProposedPosition => ExistingPositions.Any(x => x.InstrumentId == weightedProposedPosition.Instrument.InstrumentId)))
    //            UpdateOrReversePosition(cursorDate, weightedProposedPosition);
    //        return ExistingPositions;
    //    }

    //    private void UpdateOrReversePosition(DateTime cursorDate, PositionValue weightedProposedPosition)
    //    {            
    //        if (weightedProposedPosition.CurrentPrice == 0) return;
    //        var proposedPositionSize = weightedProposedPosition.ProposedWeightedPosition;
    //        var positionAdjuster = new PositionAdjuster(proposedPositionSize, ExistingPositions);
    //        var newProposedPositions = positionAdjuster.ExistingPositionsToKeep;

    //        // Close Trades
    //        foreach (var position in ExistingPositions)
    //            if (!positionAdjuster.ExistingPositionsToKeep.Any(x => x.TradeId == position.TradeId))
    //                CloseTrade(position.TradeId, cursorDate, weightedProposedPosition, weightedProposedPosition.CurrentPrice);

    //        // Open Trades
    //        if (positionAdjuster.RequiredAdjustmentToVolume != 0)
    //            OpenTrade(cursorDate, weightedProposedPosition, positionAdjuster.RequiredAdjustmentToVolume);

    //        //decimal existingPositionSize = ExistingPositions.Sum(x => x.Volume);

    //        //DealWithNetBuyPostion(cursorDate, weightedProposedPosition, existingPositionSize, proposedPositionSize);
    //        //DealWithNetSellPosition(cursorDate, weightedProposedPosition, existingPositionSize, proposedPositionSize);
    //    }

    //    public void OpenTrade(DateTime cursorDate, PositionValue weightedProposedPosition, decimal volume)
    //    {
    //        string direction = volume < 0
    //            ? PositionType.SELL.ToString()
    //            : PositionType.BUY.ToString();

    //        var stopLoss = new StopLoss(CurrentMargin,
    //            Parameters.StopLossPercent,
    //            weightedProposedPosition.Instrument.ContractUnit,
    //            volume,
    //            Parameters.ExchangeRate,
    //            volume < 0 ? PositionType.SELL : PositionType.BUY,
    //            weightedProposedPosition.CurrentPrice,
    //            weightedProposedPosition.Instrument.MinimumPriceFluctuation).StopLossInCurrency();

    //        var position = new Test_Trades
    //        {
    //            TestId = TestId,
    //            InstrumentId = weightedProposedPosition.Instrument.InstrumentId,
    //            Created = cursorDate,
    //            Direction = direction.Trim(),
    //            EntryPrice = weightedProposedPosition.CurrentPrice,
    //            Status = PositionStatus.POSITION.ToString(),
    //            Volume = volume,
    //            Instrument = weightedProposedPosition.Instrument,
    //            TakeProfit = 0,
    //            StopLoss = stopLoss,
    //            Commission = 0,
    //            Comment = "None",
    //            ClosePrice = 0,
    //            TrailingStop = 0,
    //            Margin = 0.0M,
    //            InstrumentWeight = "None",
    //            ForecastAtClose = 0.0M,
    //            ForecastAtEntry = 0.0M,
    //            CapitalAtClose = 0.0M,
    //            CapitalAtEntry = 0.0M
    //        };
    //        ExistingPositions.Add(position);
    //    }

    //    private void CloseTrade(int tradeId, DateTime cursorDate, PositionValue weightedProposedPosition, decimal currentPrice)
    //    {
    //        foreach (
    //            Test_Trades existingPosition in
    //                ExistingPositions.Where(x => x.TradeId == tradeId))
    //        {
    //            existingPosition.ClosePrice = currentPrice;
    //            existingPosition.ClosedAt = cursorDate;
    //            existingPosition.Status = PositionStatus.HISTORICALTRADE.ToString();
    //            existingPosition.Comment = "Position closed due to volume change";
    //            existingPosition.Margin =
    //                Reports.Margin.Calculate(existingPosition.Instrument.ContractUnit, Parameters.ExchangeRate, existingPosition,
    //                    currentPrice);
    //        }
    //    }

    //    private void DealWithNetSellPosition(DateTime cursorDate, PositionValue weightedProposedPosition,
    //        decimal positionSize,
    //        decimal proposedPositionSize)
    //    {
    //        if (positionSize < 0) // existing position is net sell
    //        {
    //            if (proposedPositionSize < positionSize)
    //                // Add new position to cover increased size of sell position
    //                ExistingPositions =
    //                    new CreatePositions(Parameters, TestId, ExistingPositions, CurrentMargin).CreatePosition(
    //                        cursorDate, weightedProposedPosition, positionSize);
    //            if (proposedPositionSize > positionSize)
    //                // Close existing position and open new position at reduced size
    //                CloseOutOrReducePosition(cursorDate, weightedProposedPosition,
    //                    proposedPositionSize);
    //        }
    //    }

    //    private void DealWithNetBuyPostion(DateTime cursorDate, PositionValue weightedProposedPosition,decimal existingPositionSize,decimal proposedPositionSize)
    //    {
    //        if (existingPositionSize > 0) //existing position is net buy
    //        {
    //            if (proposedPositionSize > existingPositionSize)
    //                // Add new position to cover difference
    //                ExistingPositions =
    //                    new CreatePositions(Parameters, TestId, ExistingPositions, CurrentMargin).CreatePosition(
    //                        cursorDate, weightedProposedPosition, existingPositionSize);
    //            if (proposedPositionSize < existingPositionSize)
    //                // Close existing positions and create new position at reduced size
    //                CloseOutOrReducePosition(cursorDate, weightedProposedPosition, existingPositionSize);
    //        }
    //    }

    //    private void CloseOutOrReducePosition(DateTime cursorDate, PositionValue weightedProposedPosition, decimal existingdPositionSize)
    //    {
    //        if (AnySubPositionsEqualNewPosition(ExistingPositions.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.InstrumentId).ToList(), existingdPositionSize - weightedProposedPosition.ProposedWeightedPosition) > 0)
    //        {
    //            decimal tradeId = AnySubPositionsEqualNewPosition(ExistingPositions.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.InstrumentId).ToList(),
    //                existingdPositionSize - weightedProposedPosition.ProposedWeightedPosition);
    //            foreach (Test_Trades existingPostion in ExistingPositions.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.InstrumentId).ToList())
    //            {
    //                if (existingPostion.TradeId == tradeId)
    //                {
    //                    existingPostion.ClosePrice = weightedProposedPosition.CurrentPrice;
    //                    existingPostion.ClosedAt = cursorDate;
    //                    existingPostion.Status = PositionStatus.HISTORICALTRADE.ToString();
    //                    existingPostion.Comment = "Position closed due to volume change";
    //                    existingPostion.Margin = Reports.Margin.Calculate(existingPostion.Instrument.ContractUnit, Parameters.ExchangeRate,existingPostion, weightedProposedPosition.CurrentPrice);
    //                }
    //                else
    //                {
    //                    UpdatePositionStopLoss(existingPostion, weightedProposedPosition.Instrument.MinimumPriceFluctuation, weightedProposedPosition.ProposedWeightedPosition);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            CloseOutPosition(cursorDate, weightedProposedPosition, weightedProposedPosition.CurrentPrice);
    //            var create = new CreatePositions(Parameters, TestId, ExistingPositions, CurrentMargin);
    //            var forecastlist = new List<IForecastValue> {weightedProposedPosition.ForecastValue};
    //            ExistingPositions = create.CreateNewPositions(cursorDate, new WeightedProposedPositions(forecastlist, Parameters, HistoricalDataSets, CurrentMargin));
    //        }
    //    }



    //    private void CloseOutPosition(DateTime cursorDate, PositionValue weightedProposedPosition, decimal currentPrice)
    //    {
    //        foreach (
    //            Test_Trades existingPosition in
    //                ExistingPositions.Where(x => x.InstrumentId == weightedProposedPosition.Instrument.InstrumentId))
    //        {
    //            existingPosition.ClosePrice = currentPrice;
    //            existingPosition.ClosedAt = cursorDate;
    //            existingPosition.Status = PositionStatus.HISTORICALTRADE.ToString();
    //            existingPosition.Comment = "Position closed due to volume change";
    //            existingPosition.Margin =
    //                Reports.Margin.Calculate(existingPosition.Instrument.ContractUnit, Parameters.ExchangeRate, existingPosition,
    //                    currentPrice);
    //        }
    //    }

    //    private void UpdatePositionStopLoss(Test_Trades existingPostion, decimal minimumPriceFluctuation, decimal totalPosition)
    //    {
    //        Instrument instrument = Parameters.Instruments.First(x => x.InstrumentId == existingPostion.InstrumentId);
    //        var stopLoss = new StopLoss(CurrentMargin, 
    //            Parameters.StopLossPercent,
    //            instrument.ContractUnit,
    //            totalPosition, Parameters.ExchangeRate,
    //            existingPostion.Volume < 0 ? PositionType.SELL : PositionType.BUY,
    //            existingPostion.EntryPrice,
    //            minimumPriceFluctuation);
    //        existingPostion.StopLoss = stopLoss.StopLossInCurrency();
    //    }

    //    private decimal AnySubPositionsEqualNewPosition(IEnumerable<Test_Trades> existingPostions,
    //        decimal proposedPositionSize)
    //    {
    //        foreach (Test_Trades position in existingPostions.OrderBy(x => x.Created))
    //        {
    //            if (position.Volume == proposedPositionSize)
    //                return position.TradeId;
    //        }
    //        return 0;
    //    }

    //}
}