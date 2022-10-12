using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("api/potions")]
public class PotionApiController : ControllerBase
{
    private readonly IPotionService _potionService;
    private readonly IStudentService _studentService;
    private readonly IRecipeService _recipeService;

    public PotionApiController(
        IPotionService potionService,
        IStudentService studentService,
        IRecipeService recipeService
    )
    {
        _potionService = potionService;
        _studentService = studentService;
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<IEnumerable<Potion>> GetAllPotions()
    {
        return await _potionService.GetAllPotions();
    }

    [HttpPost("{studentId}")]
    public async Task<IActionResult> AddPotion(long studentId, [FromBody] Potion potionFromStudent)
    {
        Student student = await _studentService.GetStudentById(studentId);

        // Check if Student is existing in DB with the defined studentId
        if (student is null)
        {
            return NotFound($"Student with studentId: {studentId}, does not exist in the database.");
        }

        // Numbering used during Recipe & Potion name generation
        int studentsNextRecipeNumber = await _recipeService.GetNumberOfRecipesByStudent(student) + 1;
        int studentsNextPotionNumber = await _potionService.GetNumberOfPotionsByStudent(student) + 1;


        Recipe recipeFromDb = await _recipeService.GetRecipeByIngredients(potionFromStudent.Ingredients);

        // If a Recipe with the same Ingredients exists in DB
        if (recipeFromDb is not null)
        {
            // Save Potion as Replica with already existing Recipe
            Potion potionReplica = _potionService.CreatePotion(student, BrewingStatus.Replica, recipeFromDb, studentsNextPotionNumber);
            await _potionService.SavePotionToDb(potionReplica);
            return CreatedAtAction(nameof(AddPotion), potionReplica);
        }

        // If Recipe with the same Ingredients does NOT exists in DB
        // Create new recipe based on Student's Ingredients
        Recipe recipeFromStudent = _recipeService.CreateRecipe(student, potionFromStudent.Ingredients, studentsNextRecipeNumber);

        // Save Potion to DB as Discovery
        Potion potionDiscovery = _potionService.CreatePotion(student, BrewingStatus.Discovery, recipeFromStudent, studentsNextPotionNumber);
        await _potionService.SavePotionToDb(potionDiscovery);
        
        return CreatedAtAction(nameof(AddPotion), potionDiscovery);
    }

    [HttpGet("{studentId}")]
    public async Task<List<Potion>> GetPotionsOfAStudent(long studentId)
    {
        return await _potionService.GetPotionsOfAStudent(studentId);
    }

    [HttpPost("brew")]
    public async Task<IActionResult> StartNewPotion([FromBody] Student student)
    {
        Student studentFromDb = await _studentService.GetStudentById(student.ID);

        // Check if Student is existing in DB with the defined studentId
        if (studentFromDb is null)
        {
            return NotFound($"Student with studentId: {student.ID}, does not exist in the database.");
        }
        
        Potion newPotionPersistedInDb = await _potionService.StartNewPotion(studentFromDb);

        return CreatedAtAction(nameof(StartNewPotion), newPotionPersistedInDb);
    }

    [HttpPut("{potionId}/add")]
    public async Task<IActionResult> AddIngredientToPotion(long potionId, [FromBody] Ingredient ingredient)
    {
        Potion potionFromDb = await _potionService.GetPotionById(potionId);

        // Check if Potion is existing in DB with the defined potionId
        if (potionFromDb is null)
        {
            return NotFound($"Potion with potionId: {potionId}, does not exist in the database.");
        }
        
        // Check if Potion in DB already has the Ingredient we're trying to add
        if (_potionService.IsPotionHasAnIngredient(potionFromDb, ingredient))
        {
            return BadRequest($"Duplicate Ingredient! The Potion already has \"{ingredient.Name}\" ingredient.");
        }

        bool ingredientAddedToPotion = await _potionService.AddIngredientToPotion(potionFromDb, ingredient);

        Potion updatedPotionFromDb = await _potionService.GetPotionById(potionId);
        
        // Check if the Potion has already 5 Ingredients, if so, check Recipes
        // and set it's status accordingly (Discovery/Replica)
        if (updatedPotionFromDb.Ingredients.Count == 5)
        {
            Recipe sameRecipeFromDb = await _recipeService.GetRecipeByIngredients(updatedPotionFromDb.Ingredients);
            int studentsNextRecipeNumber = await _recipeService.GetNumberOfRecipesByStudent(updatedPotionFromDb.Student) + 1;
            if (sameRecipeFromDb is not null)
            {
                await _potionService.FinalizePotionInDb(updatedPotionFromDb, BrewingStatus.Replica, studentsNextRecipeNumber);
                await _potionService.AddRecipeToPotion(updatedPotionFromDb, sameRecipeFromDb);
            }
            else
            {
                await _potionService.FinalizePotionInDb(updatedPotionFromDb, BrewingStatus.Discovery, studentsNextRecipeNumber);
                
                // If Potion is Discovery, Recipe should also be persisted
                Recipe newRecipe = _recipeService.CreateRecipe(updatedPotionFromDb.Student, updatedPotionFromDb.Ingredients,
                    studentsNextRecipeNumber);
                await _recipeService.AddRecipeToDb(newRecipe);
                await _potionService.AddRecipeToPotion(updatedPotionFromDb, newRecipe);
            }
        }
        
        if (ingredientAddedToPotion)
        {
            return CreatedAtAction(nameof(AddIngredientToPotion), updatedPotionFromDb);
        }

        return BadRequest("Cannot add more Ingredient to this Potion.");
    }

    [HttpGet("{potionId}/help")]
    public async Task<ActionResult<List<string>>> GetPossibleRecipes(long potionId)
    {
        Potion potionBrewingFromDb = await _potionService.GetPotionById(potionId);

        if (potionBrewingFromDb is null)
        {
            return NotFound($"Potion with potionId: {potionId}, does not exist in the database.");
        }

        List<Recipe> matchingRecipes = new();

        List<Ingredient> potionBrewingIngredients = potionBrewingFromDb.Ingredients.ToList();
        List<Recipe> recipesFromDb = await _recipeService.GetAllRecipes();

        foreach (Recipe recipe in recipesFromDb)
        {
            bool ingredientFound = false;
            foreach (Ingredient ingredientFromBrewing in potionBrewingIngredients)
            {
                foreach (Ingredient ingredientFromDb in recipe.Ingredients)
                {
                    if (ingredientFromBrewing.ID == ingredientFromDb.ID)
                    {
                        matchingRecipes.Add(recipe);
                        ingredientFound = true;
                        break;
                    }
                }
                
                if (ingredientFound)
                {
                    break;
                }
            }
        }

        if (matchingRecipes.Count == 0)
        {
            return NotFound("Recipe with similar ingredients does not exists in the database so far.");
        }

        return matchingRecipes
            .Select(recipe => recipe.Name)
            .ToList();
    }
}