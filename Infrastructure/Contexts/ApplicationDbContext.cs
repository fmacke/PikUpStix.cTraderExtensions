using Application;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Contexts
{
    public partial class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DBCredentialsService? dbCredentialsService = GetCredentials();
            string connString = @"Server=localhost;Database=" + dbCredentialsService.GetDbName() + ";User Id=sa;Password=" + dbCredentialsService.GetPassword() + ";Encrypt=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connString);
        }

        private static DBCredentialsService? GetCredentials()
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(AppContext.BaseDirectory)
                          .AddUserSecrets<ApplicationDbContext>();

            var configuration = builder.Build();
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<DBCredentialsService>();
            var serviceProvider = services.BuildServiceProvider();
            return  serviceProvider.GetService<DBCredentialsService>();
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
    public class DBCredentialsService
    {
        private readonly IConfiguration _configuration;
        public DBCredentialsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetPassword()
        {
            return _configuration["SqlPassword"];
        }
        public string GetDbName()
        {
            return _configuration["DbName"];
        }
    }
}
