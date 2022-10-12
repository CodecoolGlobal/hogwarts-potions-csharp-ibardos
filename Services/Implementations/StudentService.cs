using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services.Implementations;

public class StudentService : IStudentService
{
    private readonly HogwartsContext _context;

    public StudentService(HogwartsContext context)
    {
        _context = context;
    }

    
    /// <summary>
    /// Get student object from database by studentId
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns>Student object</returns>
    public async Task<Student> GetStudentById(long studentId)
    {
        return await _context
            .Students
            .FirstOrDefaultAsync(student => student.Id == studentId);
    }
}