using Application.Business.Forecasts;
using Application.Business.Volatility;
using Application.Business.PositionSize;
using Domain.Entities;
using Domain.Enums;
using Application.Business.Risk;
using Application.Business.Calculations;

namespace Application.Business.Portfolio
{
    public class PositionValue
    {
        public double UnweightedPosition { get; private set; }
        public double ProposedWeightedPosition { get; private set; }
        public double NonRoundedProposedWeightedPosition { get; private set; }
        //public double InstrumentPriceVolatility { get; private set; }
        public double StopLossAt { get; private set; }
        public double StopLossInPips { get; private set; }
        public List<PortfolioInstrument> PorfolioInstruments { get; private set; }
        public List<HistoricalData> HistoricalPriceSet { get; private set; }
        public double StopLossPercent { get; private set; }
        public double ExchangeRate { get; private set; }
        public double TargetVolatility { get; private set; }
        public double AskingPrice { get; set; }
        public double BiddingPrice { get; set; }
        public Instrument Instrument { get; set; }
        public double AvailableTradingCapital { get; set; }

        public PositionValue(IForecastValue forecast, double stopLossPercent, double exchangeRate, double targetVolatility,
            List<HistoricalData> historicalData, double availableTradingCapital, string symbol)
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
            Instrument = new Instrument()
            {
                ContractUnit = 1,
                MinimumPriceFluctuation = 0.0001,
                InstrumentName = symbol,
                Id = 1,
                Currency = "GBP"
            };
            UnweightedPosition = CalculateUnweightedPosition(forecast);
            double weightedPosition = CalculateWeightedPosition(forecast, UnweightedPosition);
            ProposedWeightedPosition = FloorCeiling(weightedPosition);
            NonRoundedProposedWeightedPosition = weightedPosition;
            if (ProposedWeightedPosition != 0)
                StopLossAt = GetStopLoss(ProposedWeightedPosition, AskingPrice, BiddingPrice);
        }

        public IForecastValue ForecastValue { get; set; }




        private double FloorCeiling(double weightedPosition)
        {
            if (weightedPosition > 0) return Math.Floor(weightedPosition);
            if (weightedPosition < 0) return Math.Ceiling(weightedPosition);
            return 0;
        }

        private double GetStopLoss(double proposedWeightedPosition, double askingPrice, double biddingPrice)
        {
            var positionType = PositionType.BUY;
            if (proposedWeightedPosition < 0)
                positionType = PositionType.SELL;

            if (proposedWeightedPosition != 0)
            {
                var sl = new StopLossAtPrice(AvailableTradingCapital, StopLossPercent,
                    Instrument.ContractUnit,
                    proposedWeightedPosition,
                    ExchangeRate,
                    positionType, askingPrice, biddingPrice,
                    Instrument.MinimumPriceFluctuation);
                StopLossInPips = sl.StopLossInPips();
                return sl.Calculate();
            }
            return 0;
        }

        private double CalculateWeightedPosition(IForecastValue ewmacForecast, double position)
        {
            double forecastDiversicationMultiplier = GetForecastDiversificationMultiplier();
            double instrumentWeight = GetInstrumentWeight(); // ewmacForecast.Instrument.Id);
            return position * forecastDiversicationMultiplier * instrumentWeight;
        }

        private double CalculateUnweightedPosition(IForecastValue foreCast)
        {
            var subSystemPosition = new SubSystemPosition(
                foreCast.Forecast, AvailableTradingCapital, TargetVolatility,
                new InstrumentPositionSize(
                    foreCast.InstrumentBlock,
                    foreCast.AskingPrice,
                    CalculateInstrumentPriceVolatility(foreCast.DateTime),
                    ExchangeRate));
            return Math.Round(subSystemPosition.GetUnscaledPosition(), 9);
        }

        public double CalculateInstrumentPriceVolatility(DateTime currentPeriodDate)
        {
            int periodsToCheck = 25;
            if (DataSetIncludesPeriodDate(currentPeriodDate))
            {
                HistoricalData currentData =
                    HistoricalPriceSet.First(
                        x =>
                            x.Date == currentPeriodDate.Date);

                int index = HistoricalPriceSet.ToList().IndexOf(currentData);
                if (SufficientPreviousDataToCalculateVolatility(index, periodsToCheck, HistoricalPriceSet.Count - 1))
                {
                    IEnumerable<HistoricalData> recentData = HistoricalPriceSet
                        .GetRange(index, periodsToCheck).Where(x => x.ClosePrice != 0);
            
                    //InstrumentPriceVolatility = PriceVolatility.Calculate(recentData.ToList(), periodsToCheck);
                    return PriceVolatility.OfClosePrices(recentData.ToList(), periodsToCheck);
                }
            }
            return 0;
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
                        x.Date == currentPeriodDate.Date);
        }

        public double GetForecastDiversificationMultiplier()
        {
            // See C:\Users\Finn\OneDrive\Documents\Business\trading\Systematic Trading - Robert Carver - Resources\Instrument Diversification Calculator.xlsx for calculation
            return 1;
            //return Convert.ToDecimal(1.652892562);
        }

        private double GetInstrumentWeight()
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