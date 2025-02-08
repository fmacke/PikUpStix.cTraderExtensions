namespace Application.Business.PositionSize
{
    public class InstrumentPositionSize
    {
        public InstrumentPositionSize(double instrumentBlock, double instrumentPrice,
            double instrumentPriceVolatility, double exchangeRate)
        {
            InstrumentBlock = instrumentBlock;
            CurrentPrice = instrumentPrice;
            PriceVolatility = instrumentPriceVolatility;
            ExchangeRate = exchangeRate;
        }

        public double InstrumentBlock { get; private set; }
        public double CurrentPrice { get; private set; }
        public double PriceVolatility { get; private set; }
        public double ExchangeRate { get; private set; }

        public double BlockValue
        {
            get { return InstrumentBlock * (CurrentPrice / 100); }
        }

        public double CurrencyVolatility
        {
            get { return BlockValue * PriceVolatility * 100; }
        }

        public double ValueVolatility
        {
            get { return CurrencyVolatility * ExchangeRate; }
        }
    }
}