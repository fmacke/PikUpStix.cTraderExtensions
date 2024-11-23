using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        using (ApplicationDbContext db = new ApplicationDbContext())
        {
            Console.Write("Nuke APPLICATIONDBContext db and start again? (y or n)");
            var name = Console.ReadLine();

            if (name.Equals("y"))
            {
                //db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                Console.Write("db has been deleted and rebuilt");
            }
        }

        Console.Write("All works complete - press any key to exit...");
    }
}
