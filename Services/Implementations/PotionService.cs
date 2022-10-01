using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services.Implementations;

public class PotionService : IPotionService
{
    private const int MaxIngredientsForPotions = 5;
    private readonly HogwartsContext _context;

    public PotionService(HogwartsContext context)
    {
        _context = context;
    }

    
    public async Task SavePotionToDb(Potion potion)
    {
        await _context.Potions.AddAsync(potion);
        await _context.SaveChangesAsync();
    }

    public async Task<Potion> StartNewPotion(Student student)
    {
        Potion newPotion = new Potion(
            name: $"{student.Name}'s brewing.",
            student: student,
            brewingStatus: BrewingStatus.Brew,
            recipe: null
            );

        await SavePotionToDb(newPotion);

        return await GetLastBrewedPotion();
    }
    
    public async Task<IEnumerable<Potion>> GetAllPotions()
    {
        return await _context
            .Potions
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Student)
            .Include(potion => potion.Student.Room)
            .Include(potion => potion.Recipe)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<int> GetNumberOfPotionsByStudent(Student student)
    {
        return await _context
            .Potions
            .AsNoTracking()
            .CountAsync(potion => potion.Student == student);
    }

    public async Task<List<Potion>> GetPotionsOfAStudent(long studentId)
    {
        return await _context
            .Potions
            .AsNoTracking()
            .Include(potion => potion.Student)
            .Include(potion => potion.Student.Room)
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Recipe)
            .Where(potion => potion.Student.ID == studentId)
            .Select(potion => potion)
            .ToListAsync();
    }

    public async Task<Potion> GetPotionById(long potionId)
    {
        return await _context
            .Potions
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Student)
            .Include(potion => potion.Recipe)
            .FirstOrDefaultAsync(potion => potion.ID == potionId);
    }

    public async Task FinalizePotionInDb(Potion potion, BrewingStatus brewingStatus, int studentsNextPotionNumber)
    {
        potion.Name = $"{potion.Student.Name}'s potion #{studentsNextPotionNumber}";
        potion.BrewingStatus = brewingStatus;
        _context.Potions.Update(potion);
        await _context.SaveChangesAsync();
    }


    // Helper methods
    public Potion CreatePotion(Student student, BrewingStatus brewingStatus, Recipe recipe, int studentsNextPotionNumber)
    {
        Potion potion = new Potion(
            name: $"{student.Name}'s potion #{studentsNextPotionNumber}",
            student: student,
            brewingStatus: brewingStatus,
            recipe: recipe
        );

        return potion;
    }

    public async Task<bool> AddIngredientToPotion(Potion potionFromDb, Ingredient ingredient)
    {
        Ingredient ingredientFromDb = await GetIngredientFromDb(ingredient);
        
        if (potionFromDb.Ingredients.Count < MaxIngredientsForPotions)
        {
            if (ingredientFromDb is not null)
            {
                potionFromDb.Ingredients.Add(ingredientFromDb);
            }
            else
            {
                potionFromDb.Ingredients.Add(ingredient);
            }

            _context.Potions.Update(potionFromDb);
            await _context.SaveChangesAsync();
            
            return true;
        }

        return false;
    }

    public bool IsPotionHasAnIngredient(Potion potion, Ingredient ingredientFromStudent)
    {
        return potion
            .Ingredients
            .Any(ingredient => ingredient.Name == ingredientFromStudent.Name);
    }

    public async Task AddRecipeToPotion(Potion potion, Recipe recipe)
    {
        potion.Recipe = recipe;
        _context.Potions.Update(potion);
        await _context.SaveChangesAsync();
    }

    private async Task<Ingredient> GetIngredientFromDb(Ingredient ingredientFromStudent)
    {
        return await _context
            .Ingredients
            .FirstOrDefaultAsync(ingredient => ingredient.Name == ingredientFromStudent.Name);
    }

    private async Task<Potion> GetLastBrewedPotion()
    {
        return await _context
            .Potions
            .AsNoTracking()
            .Include(potion => potion.Student)
            .OrderByDescending(potion => potion.ID)
            .Select(potion => potion)
            .FirstOrDefaultAsync();
    }
}