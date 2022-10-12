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


    /// <summary>
    /// Adds Potion object to database, passed as an argument
    /// </summary>
    /// <param name="potion"></param>
    /// <returns>Saved Potion object</returns>
    public async Task<Potion> AddPotionToDb(Potion potion)
    {
        Potion savedPotion = _context.Potions.Add(potion).Entity;
        await _context.SaveChangesAsync();

        return savedPotion;
    }

    /// <summary>
    /// Creates a new Potion object for the defined Student, and adds that to database
    /// </summary>
    /// <param name="student"></param>
    /// <returns>Created Potion object</returns>
    public async Task<Potion> StartNewPotion(Student student)
    {
        Potion newPotion = new Potion(
            name: $"{student.Name}'s brewing.",
            student: student,
            brewingStatus: BrewingStatus.Brew,
            recipe: null
            );

        return await AddPotionToDb(newPotion);
    }

    /// <summary>
    /// Gets Potion object from database by potionId
    /// </summary>
    /// <param name="potionId"></param>
    /// <returns>Potion object</returns>
    public async Task<Potion> GetPotionById(long potionId)
    {
        return await _context
            .Potions
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Student)
            .Include(potion => potion.Recipe)
            .FirstOrDefaultAsync(potion => potion.Id == potionId);
    }
    
    /// <summary>
    /// Gets all Potion objects from database
    /// </summary>
    /// <returns></returns>
    public async Task<List<Potion>> GetAllPotions()
    {
        return await _context
            .Potions
            .AsNoTracking()
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Student)
            .Include(potion => potion.Student.Room)
            .Include(potion => potion.Recipe)
            .ToListAsync();
    }

    /// <summary>
    /// Gets Potion objects from database, made by a Student
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns>List of Potion objects</returns>
    public async Task<List<Potion>> GetPotionsOfAStudent(long studentId)
    {
        return await _context
            .Potions
            .AsNoTracking()
            .Include(potion => potion.Student)
            .Include(potion => potion.Student.Room)
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Recipe)
            .Where(potion => potion.Student.Id == studentId)
            .Select(potion => potion)
            .ToListAsync();
    }
    
    /// <summary>
    /// Gets the number of Potion objects were made by a Student
    /// </summary>
    /// <param name="student"></param>
    /// <returns>Number of Potion objects</returns>
    public async Task<int> GetNumberOfPotionsByStudent(Student student)
    {
        return await _context
            .Potions
            .AsNoTracking()
            .CountAsync(potion => potion.Student == student);
    }
    
    /// <summary>
    /// Adds an Ingredient to a Potion object, if the number of maximum ingredients has not been exceeded
    /// then updates the Entity in the database
    /// </summary>
    /// <param name="potionFromDb"></param>
    /// <param name="ingredient"></param>
    /// <returns>True if addition happened, otherwise False</returns>
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
    
    /// <summary>
    /// Adds a Recipe to a Potion object, then updates the Entity in the database
    /// </summary>
    /// <param name="potion"></param>
    /// <param name="recipe"></param>
    public async Task AddRecipeToPotion(Potion potion, Recipe recipe)
    {
        potion.Recipe = recipe;
        _context.Potions.Update(potion);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the status and name of a Potion object in the database, when its brewing is finished
    /// </summary>
    /// <param name="potion"></param>
    /// <param name="brewingStatus"></param>
    /// <param name="studentsNextPotionNumber"></param>
    public async Task FinalizePotionInDb(Potion potion, BrewingStatus brewingStatus, int studentsNextPotionNumber)
    {
        potion.Name = $"{potion.Student.Name}'s potion #{studentsNextPotionNumber}";
        potion.BrewingStatus = brewingStatus;
        _context.Potions.Update(potion);
        await _context.SaveChangesAsync();
    }


    // Helper methods
    
    /// <summary>
    /// Creates a Potion object in memory
    /// </summary>
    /// <param name="student"></param>
    /// <param name="brewingStatus"></param>
    /// <param name="recipe"></param>
    /// <param name="studentsNextPotionNumber"></param>
    /// <returns>Created Potion object</returns>
    public Potion CreatePotionInMemory(Student student, BrewingStatus brewingStatus, Recipe recipe, int studentsNextPotionNumber)
    {
        Potion potion = new Potion(
            name: $"{student.Name}'s potion #{studentsNextPotionNumber}",
            student: student,
            brewingStatus: brewingStatus,
            recipe: recipe
        );

        return potion;
    }

    /// <summary>
    /// Returns True if Potion object has the Ingredient, passed as an argument, otherwise False
    /// </summary>
    /// <param name="potion"></param>
    /// <param name="ingredientFromStudent"></param>
    /// <returns>True if Potion has the Ingredient, otherwise False</returns>
    public bool IsPotionHasAnIngredient(Potion potion, Ingredient ingredientFromStudent)
    {
        return potion
            .Ingredients
            .Any(ingredient => ingredient.Name == ingredientFromStudent.Name);
    }

    
    // Private helper methods
    
    /// <summary>
    /// Gets an Ingredient from the database by ingredient name, passed by argument, if exists
    /// </summary>
    /// <param name="ingredientFromStudent"></param>
    /// <returns>Ingredient object if exists in database, else null</returns>
    private async Task<Ingredient> GetIngredientFromDb(Ingredient ingredientFromStudent)
    {
        return await _context
            .Ingredients
            .FirstOrDefaultAsync(ingredient => ingredient.Name == ingredientFromStudent.Name);
    }
}