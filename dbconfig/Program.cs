//using DataServices;

//public class Program
//{
//    public static async Task Main(string[] args)
//    {
//        var dataService = new DataService();
//        var result = dataService.Tests.GetAllTestsCachedAsync();
//        Console.WriteLine(result);
//    }
//}

using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

internal class Program
{

    private static void Main(string[] args)
    {
        string DBName = "TradingBE";
        string connString = @"Server=localhost;Database=" + DBName + ";User Id=sa;Password=Gogogo123!;Encrypt=True;TrustServerCertificate=True;";
        DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            Console.Write("Nuke " + DBName + " db and start again? (y or n)");
            var name = Console.ReadLine();

            if (name.Equals("y"))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                Console.Write("db has been deleted and rebuilt");
            }
        }

        Console.Write("All works complete - press any key to exit...");
    }
}