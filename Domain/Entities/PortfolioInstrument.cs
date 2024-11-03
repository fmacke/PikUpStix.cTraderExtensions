using Domain.Entities;

namespace Domain.Entities
{
    public partial class PortfolioInstrument
    {
        public int PortfolioInstrumentId { get; set; }

        public int PortfolioId { get; set; }

        public int InstrumentId { get; set; }

        public decimal InstrumentWeight { get; set; }

        public virtual Instrument Instrument { get; set; }

        public virtual Porfolio Porfolio { get; set; }
    }
}
