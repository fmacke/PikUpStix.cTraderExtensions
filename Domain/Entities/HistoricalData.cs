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

        public double OpenPrice { get; set; }

        public double ClosePrice { get; set; }

        public double LowPrice { get; set; }

        public double HighPrice { get; set; }

        public double Volume { get; set; }

        public double Settle { get; set; }

        public double OpenInterest { get; set; }

        //public virtual Instrument Instrument { get; set; }
    }
}
