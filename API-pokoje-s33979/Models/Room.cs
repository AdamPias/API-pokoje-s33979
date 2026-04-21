using System.ComponentModel.DataAnnotations;

namespace API_pokoje_s33979;

public class Room
{
   public int Id { get; set; }
   [Required]
   public string Name { get; set; }
   [Required]
   public string BuildingCode { get; set; }
   public int Floor { get; set; }
   [Range(1,int.MaxValue)]
   public int Capacity { get; set; }
   public bool HasProjector { get; set; }
   public bool IsActive { get; set; }
   
}