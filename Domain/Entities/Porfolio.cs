using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public partial class Porfolio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Porfolio()
        {
            PortfolioInstruments = new HashSet<PortfolioInstrument>();
        }

        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [StringLength(200)]
        public string PortfolioName { get; set; }

        [StringLength(1000)]
        public string PortfolioDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PortfolioInstrument> PortfolioInstruments { get; set; }
    }
}
