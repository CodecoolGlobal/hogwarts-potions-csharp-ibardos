using System;
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
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] Room roomToAdd)
        {
            await _roomService.AddRoom(roomToAdd);

            return CreatedAtAction(nameof(AddRoom), roomToAdd);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoomById(long id)
        {
            Room room = await _roomService.GetRoomById(id);

            if (room is null)
            {
                return NotFound("Room with the defined ID does not exists in the database.");
            }
            
            return room;
        }

        [HttpGet]
        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _roomService.GetAllRooms();
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
