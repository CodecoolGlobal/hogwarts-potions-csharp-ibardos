using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IRecipeService
{
    // CRUD operations - Entity Framework Core
    // Create
    Task AddRecipeToDb(Recipe recipe);

    // Read
    Task<List<Recipe>> GetAllRecipes();
    Task<Recipe> GetRecipeByIngredients(HashSet<Ingredient> ingredients);
    Task<int> GetNumberOfRecipesByStudent(Student student);

    // Update


    // Delete
    
    
    
    // Helper methods
    Recipe CreateRecipeInMemory(Student student, HashSet<Ingredient> ingredients, int studentsNextRecipeNumber);
}