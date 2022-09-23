using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
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
    
    public async Task AddRoom(Room roomToAdd)
    {
        await _context.Rooms.AddAsync(roomToAdd);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Room> GetRoomById(long roomId)
    {
        return await _context
            .Rooms
            .Include(room => room.Residents)
            .FirstOrDefaultAsync(room => room.ID == roomId);
    }
    
}