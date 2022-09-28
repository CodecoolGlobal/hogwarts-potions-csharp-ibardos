using System.Collections.Generic;
using System.Linq;
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


    public async Task AddRecipe(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
        await _context.SaveChangesAsync();
    }

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

    // TODO check whether it is needed
    public async Task<List<Recipe>> GetRecipesByStudent(Student student)
    {
        return await _context
            .Recipes
            .Where(recipe => recipe.Student == student)
            .Select(recipe => recipe)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<int> GetNumberOfRecipesByStudent(Student student)
    {
        return await _context
            .Recipes
            .AsNoTracking()
            .CountAsync(recipe => recipe.Student == student);
    }
    
    
    // Helper methods
    public Recipe CreateRecipe(Student student, HashSet<Ingredient> ingredients, int studentsNextRecipeNumber)
    {
        Recipe recipe = new Recipe(
            name: $"{student.Name}'s discovery #{studentsNextRecipeNumber}",
            student: student,
            ingredients: ingredients
        );

        return recipe;
    }
}