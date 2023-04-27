using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Core.Entities;
using ParkingLotManagement.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotManagement.Application.Interfaces
{
    public interface IParkingServices
    {
        Task<bool> Add(InOutParkingDTO parkingDTO);
        Task<bool> Update(InOutParkingDTO parkingDTO);
        Task<List<ParkedCarDTO>> GetAllAsync();
    }
}
