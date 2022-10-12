using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly HogwartsContext _context;

    public RoomService(HogwartsContext context)
    {
        _context = context;
    }
    
    
    /// <summary>
    /// Adds room object to database
    /// </summary>
    /// <param name="roomToAdd"></param>
    public async Task AddRoomToDb(Room roomToAdd)
    {
        await _context.Rooms.AddAsync(roomToAdd);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Gets room object from database by roomId
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns>Room object</returns>
    public async Task<Room> GetRoomById(long roomId)
    {
        return await _context
            .Rooms
            .AsNoTracking()
            .Include(room => room.Residents)
            .FirstOrDefaultAsync(room => room.Id == roomId);
    }
    
    /// <summary>
    /// Gets all room objects from database
    /// </summary>
    /// <returns>List of room objects</returns>
    public async Task<List<Room>> GetAllRooms()
    {
        return await _context
            .Rooms
            .AsNoTracking()
            .Include(room => room.Residents)
            .ToListAsync();
    }

    /// <summary>
    /// Gets room objects from database for rat owners
    /// where neither cats nor owls can be found
    /// </summary>
    /// <returns>List of room objects</returns>
    public Task<List<Room>> GetRoomsForRatOwners()
    {
        return _context
            .Rooms
            .AsNoTracking()
            .Include(room => room.Residents)
            .Where(room => room.Residents.All(resident => resident.PetType != PetType.Cat))
            .Where(room => room.Residents.All(resident => resident.PetType != PetType.Owl))
            .Select(room => room)
            .ToListAsync();
    }

    /// <summary>
    /// Updates room object in database
    /// </summary>
    /// <param name="room"></param>
    public async Task UpdateRoomById(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes room object from database
    /// </summary>
    /// <param name="room"></param>
    public async Task DeleteRoom(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}