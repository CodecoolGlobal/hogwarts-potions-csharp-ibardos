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
}