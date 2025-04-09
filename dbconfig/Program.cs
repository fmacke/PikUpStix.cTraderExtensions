using DataServices;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using DataImports.ForexSb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class DatabaseService
{
    private readonly IConfiguration _configuration;

    public DatabaseService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetPassword()
    {
        return _configuration["SqlPassword"];
    }
    public string GetDbName() {
        return _configuration["DbName"];
    }
    
}
internal class Program
{
    private static string Password { get; set; }
    private static string DbName { get; set; } = "TradingBE";

    private static void Main(string[] args)
    {  
        Console.WriteLine("Option 1:  Update Forex Data from files");
        Console.WriteLine("Option 99:  NUKE Database forever");
        var inputText = Console.ReadLine();

        if (inputText.Equals("1"))
        {
            Console.WriteLine("Please enter the folder path to import data from:");
            var folderPath = Console.ReadLine(); //"C:\Users\finnmackenzie\Downloads\OneDrive_2_09-04-2025"
            ImportForexDataFromFile(folderPath);
            Console.WriteLine("new instrument data imported.");
        }
        if (inputText.Equals("99"))
        {
            NukeDatabase();
            Console.WriteLine("db has been deleted and rebuilt");
        }

        Console.WriteLine("All works complete - press any key to exit...");
    }

    private static void ImportForexDataFromFile(string folderPath)
    {

        DataService dataService = new DataService();
        var historicalDataProcessor = new HistoricalDataProcessor(folderPath);
        historicalDataProcessor.ProcessFiles();
    }

    private static void NukeDatabase()
    {
        var builder = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddUserSecrets<Program>()
           .AddEnvironmentVariables();

        var configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddTransient<DatabaseService>();

        var serviceProvider = services.BuildServiceProvider();

        var myService = serviceProvider.GetService<DatabaseService>();

        Password = myService.GetPassword();
        DbName = myService.GetDbName();
        string connString = @"Server=localhost;Database=" + DbName + ";User Id=sa;Password=" + Password + ";Encrypt=True;TrustServerCertificate=True;";
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            Console.Write("Are you FUCKING CERTAIN you want to nuke " + DbName + " db and start again? (y or n)");
            var name = Console.ReadLine();

            if (name.Equals("y"))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                Console.Write("db has been deleted and rebuilt");
            }
        }
    }
}
