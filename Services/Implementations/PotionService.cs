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
    private readonly HogwartsContext _context;

    public PotionService(HogwartsContext context)
    {
        _context = context;
    }

    
    public async Task AddPotion(Potion potion)
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