using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services.Implementations;

public class RecipeService : IRecipeService
{
    private readonly HogwartsContext _context;

    public RecipeService(HogwartsContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Adds Recipe object to database, passed as an argument
    /// </summary>
    /// <param name="recipe"></param>
    public async Task AddRecipeToDb(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all Recipe objects from database
    /// </summary>
    /// <returns>List of Recipe objects</returns>
    public async Task<List<Recipe>> GetAllRecipes()
    {
        return await _context
            .Recipes
            .Include(recipe => recipe.Ingredients)
            .ToListAsync();
    }

    /// <summary>
    /// Gets a Recipe from the database, if any, which has the defined set of Ingredients
    /// </summary>
    /// <param name="ingredients"></param>
    /// <returns>Recipe object from database, or null</returns>
    public async Task<Recipe> GetRecipeByIngredients(HashSet<Ingredient> ingredients)
    {
        // Create HashSet of Ingredient names from Student to compare
        HashSet<string> ingredientNamesFromStudent = new();
        foreach (Ingredient ingredientFromStudent in ingredients)
        {
            ingredientNamesFromStudent.Add(ingredientFromStudent.Name);
        }

        // Get existing Recipes from DB to compare list of Ingredients
        List<Recipe> recipesFromDb = await _context.Recipes.Include(recipe => recipe.Ingredients).ToListAsync();

        // Iterate on Recipes and check the created HashSet of Ingredients to find equality
        foreach (Recipe recipe in recipesFromDb)
        {
            HashSet<string> ingredientNamesFromDb = new();
            foreach (Ingredient ingredientFromDb in recipe.Ingredients)
            {
                ingredientNamesFromDb.Add(ingredientFromDb.Name);
                
                // If Ingredients are matching, return Recipe
                if (ingredientNamesFromStudent.SetEquals(ingredientNamesFromDb))
                {
                    return recipe;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the number of Recipe objects were made by a Student
    /// </summary>
    /// <param name="student"></param>
    /// <returns>Number of Recipe objects</returns>
    public async Task<int> GetNumberOfRecipesByStudent(Student student)
    {
        return await _context
            .Recipes
            .AsNoTracking()
            .CountAsync(recipe => recipe.Student == student);
    }


    // Helper methods
    
    /// <summary>
    /// Creates an "in memory" Recipe object
    /// </summary>
    /// <param name="student"></param>
    /// <param name="ingredients"></param>
    /// <param name="studentsNextRecipeNumber"></param>
    /// <returns>Created "in memory" Recipe object</returns>
    public Recipe CreateRecipeInMemory(Student student, HashSet<Ingredient> ingredients, int studentsNextRecipeNumber)
    {
        Recipe recipe = new Recipe(
            name: $"{student.Name}'s discovery #{studentsNextRecipeNumber}",
            student: student,
            ingredients: ingredients
        );

        return recipe;
    }
}