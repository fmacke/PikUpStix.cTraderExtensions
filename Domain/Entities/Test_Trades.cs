using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities
{
    public partial class Test_Trades
    {
        [Key]
        public int TradeId { get; set; }

        public int TestId { get; set; }

        public int InstrumentId { get; set; }

        public decimal Volume { get; set; }

        [Required]
        [StringLength(50)]
        public string Direction { get; set; }

        public decimal EntryPrice { get; set; }

        public decimal TakeProfit { get; set; }

        public decimal StopLoss { get; set; }

        [Column(TypeName = "money")]
        public decimal Commission { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime Created { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Comment { get; set; }

        public decimal ClosePrice { get; set; }

        public int TrailingStop { get; set; }

        [Column(TypeName = "money")]
        public decimal Margin { get; set; }

        [Required]
        [StringLength(50)]
        public string InstrumentWeight { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ClosedAt { get; set; }

        [Column(TypeName = "money")]
        public decimal? CapitalAtEntry { get; set; }

        [Column(TypeName = "money")]
        public decimal? CapitalAtClose { get; set; }

        public decimal? ForecastAtEntry { get; set; }

        public decimal? ForecastAtClose { get; set; }

        public virtual Instrument Instrument { get; set; }

        public virtual Test Test { get; set; }
    }
}
