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
        await _roomService.AddRoomToDb(roomToAdd);

        return CreatedAtAction(nameof(AddRoom), roomToAdd);
    }

    [HttpGet("{roomId}")]
    public async Task<ActionResult<Room>> GetRoomById(long roomId)
    {
        Room room = await _roomService.GetRoomById(roomId);

        if (room is null)
        {
            return NotFound($"Room with roomId: {roomId}, does not exists in the database.");
        }

        return room;
    }

    [HttpGet]
    public async Task<IEnumerable<Room>> GetAllRooms()
    {
        return await _roomService.GetAllRooms();
    }

    [HttpPut("{roomId}")]
    public async Task<IActionResult> UpdateRoomById(long roomId, [FromBody] Room roomToUpdateFromFrontend)
    {
        if (roomId != roomToUpdateFromFrontend.Id)
        {
            return BadRequest($"The provided roomId: {roomId}, does not match the provided Room object's Id: {roomId}.");
        }

        Room roomToUpdateFromDb = await _roomService.GetRoomById(roomId);

        if (roomToUpdateFromDb is null)
        {
            return NotFound($"Room with roomId: {roomId}, does not exists in the database.");
        }

        await _roomService.UpdateRoomById(roomToUpdateFromFrontend);

        return NoContent();
    }

    [HttpDelete("{roomId}")]
    public async Task<IActionResult> DeleteRoomById(long roomId)
    {
        Room roomToDelete = await _roomService.GetRoomById(roomId);

        if (roomToDelete is null)
        {
            return NotFound($"Room with roomId: {roomId}, does not exists in the database.");
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