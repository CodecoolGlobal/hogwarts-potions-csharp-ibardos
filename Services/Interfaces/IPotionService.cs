using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Services.Interfaces;

public interface IPotionService
{
    // CRUD operations - Entity Framework Core
    // Create
    Task<Potion> AddPotionToDb(Potion potion);
    Task<Potion> StartNewPotion(Student student);

    // Read
    Task<Potion> GetPotionById(long potionId);
    Task<List<Potion>> GetAllPotions();
    Task<List<Potion>> GetPotionsOfAStudent(long studentId);
    Task<int> GetNumberOfPotionsByStudent(Student student);

    // Update
    Task<bool> AddIngredientToPotion(Potion potionFromDb, Ingredient ingredient);
    Task AddRecipeToPotion(Potion potion, Recipe recipe);
    Task FinalizePotionInDb(Potion potion, BrewingStatus brewingStatus, int studentsNextPotionNumber);


    // Delete
    
    
    
    // Helper methods
    Potion CreatePotionInMemory(Student student, BrewingStatus brewingStatus, Recipe recipe, int studentsNextPotionNumber);
    bool IsPotionHasAnIngredient(Potion potion, Ingredient ingredientFromStudent);
}