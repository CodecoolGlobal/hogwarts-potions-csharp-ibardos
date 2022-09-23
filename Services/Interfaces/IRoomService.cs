using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IRoomService
{
    // CRUD operations
    
    // Create
    Task AddRoom(Room room);
    
    // Read
    Task<Room> GetRoomById(long roomId);
    Task<IEnumerable<Room>> GetAllRooms();
    
    // Update
    Task UpdateRoomById(Room room);
    
    // Delete
    Task DeleteRoom(Room room);
    Task<List<Room>> GetRoomsForRatOwners();
}