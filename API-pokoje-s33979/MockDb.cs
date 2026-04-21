namespace API_pokoje_s33979;

public static class MockDb
{
    public static List<Room> Rooms { get; set; } = new List<Room>
    {
        new Room { Id = 1, Name = "Clean Code", BuildingCode = "A", Floor = 2, Capacity = 10, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Refactoring", BuildingCode = "B", Floor = 1, Capacity = 5, HasProjector = false, IsActive = true },
        new Room { Id = 3, Name = "Design Patterns", BuildingCode = "A", Floor = 3, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 4, Name = "Legacy Code", BuildingCode = "C", Floor = -1, Capacity = 30, HasProjector = true, IsActive = false }
    };

    public static List<Reservation> Reservations { get; set; } = new List<Reservation>
    {
        new Reservation { Id = 1, RoomId = 2, OrganizerName = "Anna Kowalska", Topic = "Warsztaty z HTTP i REST", Date = "2026-05-10", StartTime = "10:00:00", EndTime = "12:30:00", Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "Wzorce projektowe", Date = "2026-05-11", StartTime = "14:00:00", EndTime = "16:00:00", Status = "planned" }
    };
}