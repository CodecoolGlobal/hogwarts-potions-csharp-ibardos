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
    Task<Recipe> GetRecipeByIngredients(HashSet<Ingredient> ingredients);
    Task<int> GetNumberOfRecipesByStudent(Student student);
    Task<List<Recipe>> GetAllRecipes();

    // Update


    // Delete
    
    
    
    // Helper methods
    Recipe CreateRecipe(Student student, HashSet<Ingredient> ingredients, int studentsNextRecipeNumber);
}