using DataServices;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using DataImports.ForexSb;

internal class Program
{

    private static void Main(string[] args)
    {
        Console.Write("Option 1:  Update Forex Data from files");
        Console.Write("Option 99:  NUKE Database forever");
        var inputText = Console.ReadLine();

        if (inputText.Equals("1"))
        {
            ImportForexDataFromFile();
            Console.Write("new instrument data imported.");
        }
        if (inputText.Equals("99"))
        {
            NukeDatabase();
            Console.Write("db has been deleted and rebuilt");
        }

        Console.Write("All works complete - press any key to exit...");
    }

    private static void ImportForexDataFromFile()
    {
        DataService dataService = new DataService();
        string folderPath = @"C:\Users\finn\OneDrive\Documents\Money\Business\trading\DataImports\ForexSb";
        var historicalDataProcessor = new HistoricalDataProcessor(folderPath);
        historicalDataProcessor.ProcessFiles();
    }

    private static void NukeDatabase()
    {
        string DBName = "TradingBE";
        string connString = @"Server=localhost;Database=" + DBName + ";User Id=sa;Password=Gogogo123!;Encrypt=True;TrustServerCertificate=True;";
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            Console.Write("Are you FUCKING CERTAIN you want to nuke " + DBName + " db and start again? (y or n)");
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