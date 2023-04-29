
using System.ComponentModel.DataAnnotations;

namespace ParkingLotManagement.Domain.DTOs
{
    public class ParkedCarDTO
    {
        public string? TagNumber { get; set; }
        public DateTime EntryTime { get; set; }
        public double ElapsedTime { get; set; }
    }
}
