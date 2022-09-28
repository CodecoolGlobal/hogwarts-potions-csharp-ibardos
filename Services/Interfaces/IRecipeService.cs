using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IRecipeService
{
    // CRUD operations - Entity Framework Core
    // Create
    Task AddRecipe(Recipe recipe);

    // Read
    Task<Recipe> GetRecipeByIngredients(HashSet<Ingredient> ingredients);
    Task<List<Recipe>> GetRecipesByStudent(Student student);
    Task<int> GetNumberOfRecipesByStudent(Student student);

    // Update


    // Delete
    
    
    
    // Helper methods
    Recipe CreateRecipe(Student student, HashSet<Ingredient> ingredients, int studentsNextRecipeNumber);
}