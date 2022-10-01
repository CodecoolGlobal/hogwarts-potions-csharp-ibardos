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
    Task<Potion> GetPotionById(long potionId);

    // Update
    Task FinalizePotionInDb(Potion potion, BrewingStatus brewingStatus, int studentsNextPotionNumber);

    // Delete
    
    
    
    // Helper methods
    Potion CreatePotion(Student student, BrewingStatus brewingStatus, Recipe recipe, int studentsNextPotionNumber);
    Task<bool> AddIngredientToPotion(Potion potionFromDb, Ingredient ingredient);
    bool IsPotionHasAnIngredient(Potion potion, Ingredient ingredientFromStudent);
    Task AddRecipeToPotion(Potion potion, Recipe recipe);
}