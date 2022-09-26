using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers;

[ApiController, Route("api/room")]
public class RoomApiController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomApiController(IRoomService roomService)
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
    public async Task<IActionResult> UpdateRoomById(long id, [FromBody] Room roomToUpdateFromFrontend)
    {
        if (id != roomToUpdateFromFrontend.ID)
        {
            return BadRequest("The provided Id is not correlates with the provided Room's Id.");
        }

        Room roomToUpdateFromDb = await _roomService.GetRoomById(id);

        if (roomToUpdateFromDb is null)
        {
            return NotFound("Room with the defined ID does not exists in the database.");
        }

        await _roomService.UpdateRoomById(roomToUpdateFromFrontend);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoomById(long id)
    {
        Room roomToDelete = await _roomService.GetRoomById(id);

        if (roomToDelete is null)
        {
            return NotFound("Room with the defined ID does not exists in the database.");
        }

        await _roomService.DeleteRoom(roomToDelete);

        return NoContent();
    }

    [HttpGet("rat-owners")]
    public async Task<IEnumerable<Room>> GetRoomsForRatOwners()
    {
        return await _roomService.GetRoomsForRatOwners();
    }
}