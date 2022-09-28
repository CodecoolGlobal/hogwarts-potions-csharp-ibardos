using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("api/potion")]
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
            await _potionService.AddPotion(potionReplica);
            return CreatedAtAction(nameof(AddPotion), potionReplica);
        }

        // If Recipe with the same Ingredients does NOT exists in DB
        // Create new recipe based on Student's Ingredients
        Recipe recipeFromStudent = _recipeService.CreateRecipe(student, potionFromStudent.Ingredients, studentsNextRecipeNumber);

        // Save Potion to DB as Discovery
        Potion potionDiscovery = _potionService.CreatePotion(student, BrewingStatus.Discovery, recipeFromStudent, studentsNextPotionNumber);
        await _potionService.AddPotion(potionDiscovery);
        
        return CreatedAtAction(nameof(AddPotion), potionDiscovery);
    }
}