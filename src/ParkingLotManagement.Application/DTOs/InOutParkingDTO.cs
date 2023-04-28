using System.ComponentModel.DataAnnotations;


namespace ParkingLotManagement.Application.DTOs
{
    public class InOutParkingDTO
    {
        [Required]
        public string TagNumber { get; set; }
    }
}
