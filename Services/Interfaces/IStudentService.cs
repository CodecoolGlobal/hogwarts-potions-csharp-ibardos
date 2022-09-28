using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IStudentService
{
    // CRUD operations - Entity Framework Core
    // Create


    // Read
    Task<Student> GetStudentById(long studentId);

    // Update


    // Delete

}