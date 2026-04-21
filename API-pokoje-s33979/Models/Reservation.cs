using System.ComponentModel.DataAnnotations;
namespace API_pokoje_s33979;

public class Reservation
{
    
        public int Id { get; set; }
        public int RoomId { get; set; }
        [Required]
        public string OrganizerName { get; set; }
        [Required]
        public string Topic { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }
   
    
}