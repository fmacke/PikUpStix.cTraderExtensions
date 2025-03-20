using Application;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = @"Server=localhost;Database=TradingBE;User Id=sa;Password=Gogogo123!;Encrypt=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connString);
        }
        public virtual DbSet<ErrorMessage> ErrorMessages { get; set; }
        public virtual DbSet<HistoricalData> HistoricalData { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }
        public virtual DbSet<Porfolio> Portfolios { get; set; }
        public virtual DbSet<PortfolioInstrument> PortfolioInstruments { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Test_Parameter> Test_Parameters { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Instrument>()
               .HasMany(b => b.HistoricalDatas)
               .WithOne()
               .HasForeignKey(p => p.InstrumentId);           

            modelBuilder.Entity<Porfolio>()
                .HasMany(e => e.PortfolioInstruments)
                .WithOne(e => e.Porfolio)
                .HasForeignKey(e => e.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();          
        }
    }
}
