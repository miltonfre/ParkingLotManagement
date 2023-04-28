using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Validators;
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
        Task<OperationResult> Add(InOutParkingDTO parkingDTO);
        Task<OperationResult> Update(InOutParkingDTO parkingDTO);
        Task<Parking> GetLastParkingByTag(InOutParkingDTO parking);
        Task<List<ParkedCarDTO>> GetAllAsync();
    }
}
