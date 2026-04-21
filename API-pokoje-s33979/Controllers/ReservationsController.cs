using Microsoft.AspNetCore.Mvc;

namespace API_pokoje_s33979.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    // GET: /api/reservations?date=2026-05-10&status=confirmed&roomId=2
    [HttpGet]
    public IActionResult GetReservations([FromQuery] string? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var reservations = MockDb.Reservations.AsQueryable();
        
        if (!string.IsNullOrEmpty(date))
        {
            reservations = reservations.Where(r => r.Date == date);
        }
        if (!string.IsNullOrEmpty(status))
        {
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }
        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }

        return Ok(reservations.ToList());
    }

    // GET: /api/reservations/1
    [HttpGet("{id}")]
    public IActionResult GetReservation(int id)
    {
        var reservation = MockDb.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
        {
            return NotFound($"Reservation with id {id} not found.");
        }

        return Ok(reservation);
    }

    // POST: /api/reservations
    [HttpPost]
    public IActionResult CreateReservation([FromBody] Reservation newReservation)
    {
        var startTime = TimeOnly.Parse(newReservation.StartTime);
        var endTime = TimeOnly.Parse(newReservation.EndTime);

        if (endTime <= startTime)
        {
            return BadRequest("EndTime must be later than StartTime.");
        }
        
        var room = MockDb.Rooms.FirstOrDefault(r => r.Id == newReservation.RoomId);
        if (room == null)
        {
            return BadRequest($"Room with id {newReservation.RoomId} does not exist."); // 400
        }
        if (!room.IsActive)
        {
            return BadRequest($"Room with id {newReservation.RoomId} is currently inactive."); // 400
        }
        
        var isConflict = MockDb.Reservations.Any(r => 
            r.RoomId == newReservation.RoomId && 
            r.Date == newReservation.Date &&
            r.Id != newReservation.Id &&
            TimeOnly.Parse(newReservation.StartTime) < TimeOnly.Parse(r.EndTime) && 
            TimeOnly.Parse(newReservation.EndTime) > TimeOnly.Parse(r.StartTime));

        if (isConflict)
        {
            return Conflict("The room is already booked for the selected time."); // 409
        }
        
        newReservation.Id = MockDb.Reservations.Any() ? MockDb.Reservations.Max(r => r.Id) + 1 : 1;
        MockDb.Reservations.Add(newReservation);

        return CreatedAtAction(nameof(GetReservation), new { id = newReservation.Id }, newReservation);
    }

    // PUT: /api/reservations/1
    [HttpPut("{id}")]
    public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
    {
        var existing = MockDb.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing == null)
        {
            return NotFound($"Reservation with id {id} not found.");
        }
        
        var startTime = TimeOnly.Parse(updatedReservation.StartTime);
        var endTime = TimeOnly.Parse(updatedReservation.EndTime);

        if (endTime <= startTime) return BadRequest("EndTime must be later than StartTime.");

        var room = MockDb.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room == null) return BadRequest("Room does not exist.");
        if (!room.IsActive) return BadRequest("Room is inactive.");

        var isConflict = MockDb.Reservations.Any(r => 
            r.RoomId == updatedReservation.RoomId && 
            r.Date == updatedReservation.Date &&
            r.Id != id &&
            TimeOnly.Parse(updatedReservation.StartTime) < TimeOnly.Parse(r.EndTime) && 
            TimeOnly.Parse(updatedReservation.EndTime) > TimeOnly.Parse(r.StartTime));

        if (isConflict) return Conflict("The room is already booked for the selected time.");
        
        existing.RoomId = updatedReservation.RoomId;
        existing.OrganizerName = updatedReservation.OrganizerName;
        existing.Topic = updatedReservation.Topic;
        existing.Date = updatedReservation.Date;
        existing.StartTime = updatedReservation.StartTime;
        existing.EndTime = updatedReservation.EndTime;
        existing.Status = updatedReservation.Status;

        return Ok(existing);
    }

    // DELETE: /api/reservations/1
    [HttpDelete("{id}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = MockDb.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
        {
            return NotFound($"Reservation with id {id} not found.");
        }

        MockDb.Reservations.Remove(reservation);
        
        return NoContent();
    }
}