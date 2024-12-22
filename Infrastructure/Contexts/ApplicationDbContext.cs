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
        public virtual DbSet<Test_AnnualReturns> Test_AnnualReturns { get; set; }
        public virtual DbSet<Test_Trades> Test_Trades { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Test_Parameter> Test_Parameters { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ErrorMessage>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorMessage>()
                .Property(e => e.ClassName)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorMessage>()
                .Property(e => e.MethodName)
                .IsUnicode(false);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.OpenPrice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.ClosePrice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.LowPrice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.HighPrice)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.Volume)
                .HasPrecision(18, 0);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.Settle)
                .HasPrecision(18, 5);

            modelBuilder.Entity<HistoricalData>()
                .Property(e => e.OpenInterest)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.InstrumentName)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.Provider)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.DataName)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.DataSource)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.Format)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.Sort)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.ContractUnit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.ContractUnitType)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.PriceQuotation)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.MinimumPriceFluctuation)
                .HasPrecision(18, 5);

            modelBuilder.Entity<Instrument>()
                .Property(e => e.Currency)
                .IsUnicode(false);

            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.HistoricalDatas);
                //.WithOne(e => e.Instrument)
                //.HasForeignKey(h => h.InstrumentId)
                //.OnDelete(DeleteBehavior.Restrict)
                //.IsRequired(); 

            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.PortfolioInstruments)
                .WithOne(e => e.Instrument)
                .HasForeignKey(h => h.InstrumentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.Test_Trades)
                .WithOne(e => e.Instrument)
                .HasForeignKey(h => h.InstrumentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Porfolio>()
                .Property(e => e.PortfolioName)
                .IsUnicode(false);

            modelBuilder.Entity<Porfolio>()
                .Property(e => e.PortfolioDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Porfolio>()
                .HasMany(e => e.PortfolioInstruments)
                .WithOne(e => e.Porfolio)
                .HasForeignKey(e => e.PortfolioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<PortfolioInstrument>()
                .Property(e => e.InstrumentWeight)
                .HasPrecision(2, 2);

            modelBuilder.Entity<Test_AnnualReturns>()
                .Property(e => e.ReturnInCash)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_AnnualReturns>()
                .Property(e => e.ReturnAsPercentofInvestmentCapital)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Volume)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Direction)
                .IsUnicode(false);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.EntryPrice)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.TakeProfit)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.StopLoss)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Commission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.ClosePrice)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Margin)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.InstrumentWeight)
                .IsUnicode(false);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.CapitalAtEntry)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.CapitalAtClose)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.ForecastAtEntry)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test_Trades>()
                .Property(e => e.ForecastAtClose)
                .HasPrecision(20, 8);

            modelBuilder.Entity<Test>()
                .Property(e => e.StartingCapital)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.EndingCapital)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Test>()
                .Property(e => e.NetProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.Commission)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.LargestWinningTrade)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.LargestLosingTrades)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageTrade)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.NetShortProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.NetLongProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossShortProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossLongProfit)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.NetShortLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.NetLongLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossShortLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.GrossLongLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageWin)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageWinLong)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageWinShort)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageLoss)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageLossLong)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .Property(e => e.AverageLossShort)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Test_AnnualReturns)
                .WithOne(e => e.Test)
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Test_Trades)
                .WithOne(e => e.Test)
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Test_Parameters)
                .WithOne(e => e.Test)
                .HasForeignKey(e => e.TestId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Test_Parameter>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Test_Parameter>()
                .Property(e => e.Value)
                .IsUnicode(false);
        }
    }
}
