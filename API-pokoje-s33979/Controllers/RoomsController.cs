using Microsoft.AspNetCore.Mvc;

namespace API_pokoje_s33979.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var rooms = MockDb.Rooms.AsQueryable();
        
        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }
        
        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly.HasValue && activeOnly.Value)
        {
            rooms = rooms.Where(r => r.IsActive);
        }

        return Ok(rooms.ToList());
    }
    
    [HttpGet("{id}")]
    public IActionResult GetRoom(int id)
    {
        var room = MockDb.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
        {
            return NotFound($"Room with id {id} not found."); // Zwracamy 404
        }

        return Ok(room); // Zwracamy 200 OK
    }
    
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetRoomsByBuilding(string buildingCode)
    {
        var roomsInBuilding = MockDb.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(roomsInBuilding);
    }
    
    [HttpPost]
    public IActionResult AddRoom([FromBody] Room newRoom)
    {
        
        var newId = MockDb.Rooms.Any() ? MockDb.Rooms.Max(r => r.Id) + 1 : 1;
        newRoom.Id = newId;

        MockDb.Rooms.Add(newRoom);
        
        return CreatedAtAction(nameof(GetRoom), new { id = newRoom.Id }, newRoom);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = MockDb.Rooms.FirstOrDefault(r => r.Id == id);
        if (existingRoom == null)
        {
            return NotFound($"Room with id {id} not found.");
        }
        
        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = MockDb.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
        {
            return NotFound($"Room with id {id} not found.");
        }
        
        var hasReservations = MockDb.Reservations.Any(res => res.RoomId == id);
        if (hasReservations)
        {
            return Conflict($"Cannot delete room with id {id} because it has existing reservations.");
        }

        MockDb.Rooms.Remove(room);
        
        return NoContent();
    }
    
}