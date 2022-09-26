using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("api/potion")]
public class PotionApiController : ControllerBase
{
    private readonly IPotionService _potionService;

    public PotionApiController(IPotionService potionService)
    {
        _potionService = potionService;
    }

    [HttpGet]
    public async Task<IEnumerable<Potion>> GetAllPotions()
    {
        return await _potionService.GetAllPotions();
    }
}