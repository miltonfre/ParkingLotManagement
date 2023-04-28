using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotManagement.Application.Interfaces
{
    public interface ICustomConfigureServices
    {
         int CapacitySpots();
         decimal HourlyFee();
    }
}
