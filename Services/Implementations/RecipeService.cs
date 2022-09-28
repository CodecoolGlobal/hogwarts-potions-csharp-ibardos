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

}