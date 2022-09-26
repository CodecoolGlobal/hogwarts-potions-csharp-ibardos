using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IPotionService
{
    // CRUD operations

    // Create


    // Read
    Task<IEnumerable<Potion>> GetAllPotions();

    // Update


    // Delete
    
}