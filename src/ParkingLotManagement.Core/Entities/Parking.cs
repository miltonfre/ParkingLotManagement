using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
