using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IRoomService
{
    // CRUD operations - Entity Framework Core
    // Create
    Task AddRoomToDb(Room room);
    
    // Read
    Task<Room> GetRoomById(long roomId);
    Task<List<Room>> GetAllRooms();
    Task<List<Room>> GetRoomsForRatOwners();
    
    // Update
    Task UpdateRoomById(Room room);
    
    // Delete
    Task DeleteRoom(Room room);
}