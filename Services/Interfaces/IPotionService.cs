using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Services.Interfaces;

public interface IPotionService
{
    // CRUD operations - Entity Framework Core
    // Create
    Task SavePotionToDb(Potion potion);
    Task<Potion> StartNewPotion(Student student);

    // Read
    Task<IEnumerable<Potion>> GetAllPotions();
    Task<int> GetNumberOfPotionsByStudent(Student student);
    Task<List<Potion>> GetPotionsOfAStudent(long studentId);

    // Update


    // Delete
    
    
    
    // Helper methods
    Potion CreatePotion(Student student, BrewingStatus brewingStatus, Recipe recipe, int studentsNextPotionNumber);
}