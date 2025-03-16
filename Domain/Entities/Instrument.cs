using Domain.Abstractions;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public partial class Instrument : BaseEntity
    {
        public Instrument()
        {
            HistoricalDatas = new HashSet<HistoricalData>();
        }
        public string InstrumentName { get; set; }
        public string Provider { get; set; }
        public string DataName { get; set; }
        public string DataSource { get; set; }
        public string Format { get; set; }       
        public string Frequency { get; set; }
        public string Sort { get; set; }
        public double ContractUnit { get; set; }
        public string ContractUnitType { get; set; }
        public string PriceQuotation { get; set; }
        public double MinimumPriceFluctuation { get; set; }
        public string Currency { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }
    }
}
