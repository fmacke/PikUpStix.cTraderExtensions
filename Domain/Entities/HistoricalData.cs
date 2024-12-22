using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    [Table("HistoricalData")]
    public partial class HistoricalData
    {
        [Key]
        public int DataId { get; set; }

        //public int InstrumentId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Date { get; set; }

        public decimal? OpenPrice { get; set; }

        public decimal? ClosePrice { get; set; }

        public decimal? LowPrice { get; set; }

        public decimal? HighPrice { get; set; }

        public decimal? Volume { get; set; }

        public decimal? Settle { get; set; }

        public decimal? OpenInterest { get; set; }

        //public virtual Instrument Instrument { get; set; }
    }
}
