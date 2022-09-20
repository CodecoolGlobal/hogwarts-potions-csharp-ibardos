using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("api/room")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {

        [HttpGet]
        public async Task<List<Room>> GetAllRooms()
        {
            return await _context.GetAllRooms();
            _roomService = roomService;
        }

        [HttpPost]
        public async  Task AddRoom([FromBody] Room room)
        {
            await _context.AddRoom(room);
        }

        [HttpGet("{id}")]
        public async Task<Room> GetRoomById(long id)
        {
            return await _context.GetRoom(id);
        }

        [HttpPut("{id}")]
        public void UpdateRoomById(long id, [FromBody] Room updatedRoom)
        {
            _context.Update(updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task DeleteRoomById(long id)
        {
            await _context.DeleteRoom(id);
        }

        [HttpGet("rat-owners")]
        public async Task<List<Room>> GetRoomsForRatOwners()
        {
            return await _context.GetRoomsForRatOwners();
        }
    }
}
