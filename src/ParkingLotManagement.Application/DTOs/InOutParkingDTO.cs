using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotManagement.Application.DTOs
{
    public class InOutParkingDTO
    {
        [Required]
        public string TagNumber { get; set; }
    }
}
