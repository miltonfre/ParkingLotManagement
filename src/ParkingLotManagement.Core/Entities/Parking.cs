using System.ComponentModel.DataAnnotations;


namespace ParkingLotManagement.Core.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        [Required, StringLength(10)]
        public string TagNumber { get; set; }
        [Required, StringLength(10)]
        public DateTime EntryTime { get; set; }

        public Decimal? FeePaid { get; set; }
        public DateTime? ExitTime { get; set; }
    }
}
