namespace Application.Business.PositionSize
{
    public class InstrumentPositionSize
    {
        public InstrumentPositionSize(decimal instrumentBlock, decimal instrumentPrice,
            decimal instrumentPriceVolatility, decimal exchangeRate)
        {
            InstrumentBlock = instrumentBlock;
            CurrentPrice = instrumentPrice;
            PriceVolatility = instrumentPriceVolatility;
            ExchangeRate = exchangeRate;
        }

        public decimal InstrumentBlock { get; private set; }
        public decimal CurrentPrice { get; private set; }
        public decimal PriceVolatility { get; private set; }
        public decimal ExchangeRate { get; private set; }

        public decimal BlockValue
        {
            get { return InstrumentBlock * (CurrentPrice / 100); }
        }

        public decimal CurrencyVolatility
        {
            get { return BlockValue * PriceVolatility * 100; }
        }

        public decimal ValueVolatility
        {
            get { return CurrencyVolatility * ExchangeRate; }
        }
    }
}