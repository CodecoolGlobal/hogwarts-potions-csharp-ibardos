using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Data;

public class DbInitializer
{
    public static void Initialize(HogwartsContext context)
    {
        context.Database.EnsureCreated();
        
        // Initial data addition comes here
        // context.SaveChanges();
    }
}