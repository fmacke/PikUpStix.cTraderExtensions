using Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public partial class Instrument
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Instrument()
        {
            HistoricalDatas = new HashSet<HistoricalData>();
            PortfolioInstruments = new HashSet<PortfolioInstrument>();
            Test_Trades = new HashSet<Test_Trades>();
        }

        public int InstrumentId { get; set; }

        [Required]
        [StringLength(200)]
        public string InstrumentName { get; set; }

        [Required]
        [StringLength(200)]
        public string Provider { get; set; }

        [Required]
        [StringLength(100)]
        public string DataName { get; set; }

        [Required]
        [StringLength(50)]
        public string DataSource { get; set; }

        [Required]
        [StringLength(50)]
        public string Format { get; set; }

        [Required]
        [StringLength(50)]
        public string Frequency { get; set; }

        [Required]
        [StringLength(50)]
        public string Sort { get; set; }

        public decimal ContractUnit { get; set; }

        [StringLength(200)]
        public string ContractUnitType { get; set; }

        [StringLength(500)]
        public string PriceQuotation { get; set; }

        public decimal MinimumPriceFluctuation { get; set; }

        [Required]
        [StringLength(50)]
        public string Currency { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PortfolioInstrument> PortfolioInstruments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_Trades> Test_Trades { get; set; }
    }
}
