using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public interface IApplicationDbContext
    {
        DbSet<Test> Tests { get; set; }
        DbSet<HistoricalData> HistoricalData { get; set; }
        DbSet<Instrument> Instruments { get; set; }
        DbSet<Porfolio> Portfolios { get; set; }
        DbSet<PortfolioInstrument> PortfolioInstruments { get; set; }
        DbSet<Test_AnnualReturns> Test_AnnualReturns { get; set; }
        DbSet<Test_Parameter> Test_Parameters { get; set; }
        DbSet<Test_Trades> Test_Trades{ get; set; }
    }
}
