using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services.Implementations;

public class PotionService : IPotionService
{
    private readonly HogwartsContext _context;

    public PotionService(HogwartsContext context)
    {
        _context = context;
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
}