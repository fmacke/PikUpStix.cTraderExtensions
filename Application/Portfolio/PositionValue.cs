using Application.PositionSize;
using Application.Volatility;
using Domain.Entities;
using Domain.Enums;
using PikUpStix.Trading.Forecast;

namespace Application.Portfolio
{
    public class PositionValue
    {
        public PositionValue(IForecastValue forecast, decimal stopLossPercent, decimal exchangeRate, decimal targetVolatility,
            List<HistoricalData> historicalData, decimal availableTradingCapital)
        {
            ForecastValue = forecast;
            HistoricalPriceSet = historicalData.OrderByDescending(x => x.Date).ToList();
            //TradingSystemParamaters = parameters;
            StopLossPercent = stopLossPercent;
            ExchangeRate = exchangeRate; 
            TargetVolatility = targetVolatility;
            AvailableTradingCapital = availableTradingCapital;
            AskingPrice = forecast.AskingPrice;
            BiddingPrice = forecast.BiddingPrice;
            //PorfolioInstruments = parameters.PorfolioInstruments;
            Instrument = historicalData.First().Instrument;
            UnweightedPosition = CalculateUnweightedPosition(forecast);
            decimal weightedPosition = CalculateWeightedPosition(forecast, UnweightedPosition);
            ProposedWeightedPosition = FloorCeiling(weightedPosition);
            NonRoundedProposedWeightedPosition = weightedPosition;
            if (ProposedWeightedPosition != 0)
                StopLossAt = GetStopLoss(ProposedWeightedPosition, AskingPrice, BiddingPrice);
        }

        public IForecastValue ForecastValue { get; set; }


        public decimal UnweightedPosition { get; private set; }
        public decimal ProposedWeightedPosition { get; private set; }
        public decimal NonRoundedProposedWeightedPosition { get; private set; }
        public decimal InstrumentPriceVolatility { get; private set; }
        public decimal StopLossAt { get; private set; }
        public decimal StopLossInPips { get; private set; }
        public List<PortfolioInstrument> PorfolioInstruments { get; private set; }
        public List<HistoricalData> HistoricalPriceSet { get; private set; }
        public decimal StopLossPercent { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public decimal TargetVolatility { get; private set; }
        public double AskingPrice { get; set; }
        public double BiddingPrice { get; set; }
        public Instrument Instrument { get; set; }
        public decimal AvailableTradingCapital { get; set; }

        private decimal FloorCeiling(decimal weightedPosition)
        {
            if (weightedPosition > 0) return Math.Floor(weightedPosition);
            if (weightedPosition < 0) return Math.Ceiling(weightedPosition);
            return 0;
        }

        private decimal GetStopLoss(decimal proposedWeightedPosition, double askingPrice, double biddingPrice)
        {
            var positionType = PositionType.BUY;
            if (proposedWeightedPosition < 0)
                positionType = PositionType.SELL;

            if (proposedWeightedPosition != 0)
            {
                var sl = new StopLoss(AvailableTradingCapital, StopLossPercent,
                    Instrument.ContractUnit,
                    proposedWeightedPosition,
                    ExchangeRate,
                    positionType, askingPrice, biddingPrice,
                    Instrument.MinimumPriceFluctuation);
                StopLossInPips = sl.StopLossInPips();
                return sl.StopLossInCurrency();
            }
            return 0;
        }

        private decimal CalculateWeightedPosition(IForecastValue ewmacForecast, decimal position)
        {
            decimal forecastDiversicationMultiplier = GetForecastDiversificationMultiplier();
            decimal instrumentWeight = GetInstrumentWeight(ewmacForecast.Instrument.InstrumentId);
            return position * forecastDiversicationMultiplier * instrumentWeight;
        }

        private decimal CalculateUnweightedPosition(IForecastValue foreCast)
        {
            PriceVolatility instrumentPriceVolatility = CalculateInstrumentPriceVolatility(foreCast.Instrument.InstrumentId,
                foreCast.DateTime);
            var subSystemPosition = new SubSystemPosition(
                foreCast.Forecast, AvailableTradingCapital, TargetVolatility,
                new InstrumentPositionSize(
                    foreCast.InstrumentBlock,
                    Convert.ToDecimal(foreCast.AskingPrice),
                    Convert.ToDecimal(instrumentPriceVolatility.StandardDeviation),
                    ExchangeRate));
            return decimal.Round(subSystemPosition.GetUnscaledPosition(), 9);
        }

        public PriceVolatility CalculateInstrumentPriceVolatility(int instrumentId, DateTime currentPeriodDate)
        {
            int periodsToCheck = 25;
            if (DataSetIncludesPeriodDate(currentPeriodDate))
            {
                HistoricalData currentData =
                    HistoricalPriceSet.First(
                        x =>
                            x.Date.Value.Date == currentPeriodDate.Date);

                int index = HistoricalPriceSet.ToList().IndexOf(currentData);
                if (SufficientPreviousDataToCalculateVolatility(index, periodsToCheck, HistoricalPriceSet.Count - 1))
                {
                    IEnumerable<HistoricalData> recentData = HistoricalPriceSet
                        .GetRange(index, periodsToCheck).Where(x => x.ClosePrice != 0);
                    var pv = new PriceVolatility(recentData.ToList(), periodsToCheck);
                    InstrumentPriceVolatility = Convert.ToDecimal(pv.StandardDeviation);
                    return pv;
                }
            }
            var pev = new PriceVolatility(new List<HistoricalData>(), 0);
            InstrumentPriceVolatility = Convert.ToDecimal(pev.StandardDeviation);
            return pev;
        }

        private bool SufficientPreviousDataToCalculateVolatility(int index, int periodsToCheck, int maxCount)
        {
            return index + periodsToCheck < maxCount;
        }

        private bool DataSetIncludesPeriodDate(DateTime currentPeriodDate)
        {
            return
                HistoricalPriceSet.Any(
                    x =>
                        x.Date.Value.Date == currentPeriodDate.Date);
        }

        public decimal GetForecastDiversificationMultiplier()
        {
            // See C:\Users\Finn\OneDrive\Documents\Business\trading\Systematic Trading - Robert Carver - Resources\Instrument Diversification Calculator.xlsx for calculation
            return Convert.ToDecimal(1);
            //return Convert.ToDecimal(1.652892562);
        }

        private decimal GetInstrumentWeight(int instrumentId)
        {
            return 1;
            //return PorfolioInstruments.First(x => x.InstrumentId == instrumentId).InstrumentWeight;
            // Hardcoded method for now
            //switch (instrumentId)
            //{
            //    case 1:
            //        return Convert.ToDecimal(0.33);
            //    case 2:
            //    case 3:
            //    case 5:
            //        return Convert.ToDecimal(0.11);
            //    case 4:
            //        return Convert.ToDecimal(0.33);
            //    default:
            //        var err = new Exception("Hard coded weighting is failing GetInstrumentWeight()");
            //        new ErrorHandler("TradeSimulate", "GetInstrumentWeight", err);
            //        throw err;
            //}
        }
    }
}