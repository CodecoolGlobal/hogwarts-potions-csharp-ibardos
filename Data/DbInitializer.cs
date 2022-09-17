using System.Collections.Generic;
using System.Linq;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Data;

public static class DbInitializer
{
    public static void Initialize(HogwartsContext context)
    {
        context.Database.EnsureCreated();

        // Check if SQL tables are exist, if so, no action needed
        if (context.Students.Any() || context.Rooms.Any())
        {
            return;
        }
        
        
        // Initial data addition comes here
        // context.SaveChanges();
    }
}