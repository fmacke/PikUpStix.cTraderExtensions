using Application.Common.Results;
using Domain.Entities;
using MediatR;

namespace Application.Features.Instruments.Queries.GetById
{
    public class GetInstrumentByIdResponse : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public string Provider { get; set; }
        public string DataName { get; set; }
        public string DataSource { get; set; }
        public string Format { get; set; }
        public string Frequency { get; set; }
        public string Sort { get; set; }
        public decimal ContractUnit { get; set; }
        public string ContractUnitType { get; set; }
        public string PriceQuotation { get; set; }
        public decimal MinimumPriceFluctuation { get; set; }
        public string Currency { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }
        public virtual ICollection<PortfolioInstrument> PortfolioInstruments { get; set; }
        public virtual ICollection<Position> Test_Trades { get; set; }
    }
}
