using AutoMapper;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Application.Validators;
using ParkingLotManagement.Core.Entities;
using ParkingLotManagement.Domain.DTOs;
using ParkingLotManagement.Infrastructure.Repositories;
using System.Data.SqlClient;

namespace ParkingLotManagement.Application.Services
{
    public class ParkingServices : IParkingServices
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly ICustomConfigureServices _customConfigureServices;
         private readonly IMapper mapper;
        public ParkingServices(IParkingRepository parkingRepository, IMapper mapper, ICustomConfigureServices customConfigureServices)
        {
            _parkingRepository = parkingRepository;
            _customConfigureServices = customConfigureServices;
            this.mapper = mapper;
        }

        public async Task<List<ParkedCarDTO>> GetAllAsync()
        {
         var parked=await _parkingRepository.GetAllAsync();
            return mapper.Map<List<ParkedCarDTO>>(parked);
        }

        public async Task<OperationResult> Add(InOutParkingDTO parking)
        {
            var resultOperation = new OperationResult();
            if (string.IsNullOrEmpty(parking.TagNumber) || string.IsNullOrWhiteSpace(parking.TagNumber))
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "Tag Number can't be null";
                return resultOperation;
            }
            var result = await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
            if (result != null && result.ExitTime == null)
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "the vehicle has not entered";
                return resultOperation;
            }
            var parkingAdd=mapper.Map<Parking>(parking);
            var success = await _parkingRepository.Add(parkingAdd);
            resultOperation.IsValid = success;
            resultOperation.Message = "Successful";
            return resultOperation;
        }
        public async Task<Parking> GetLastParkingByTag(InOutParkingDTO parking)
        {
            return await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
        }
        private decimal AmmountToPay(Parking parking)
        {
            var hourlyFee = _customConfigureServices.HourlyFee();
            TimeSpan totalHoursParked = parking.EntryTime - DateTime.Now;
            var upperHour = (Math.Ceiling((decimal)totalHoursParked.TotalHours) + 1);
            return upperHour  * hourlyFee;
        } 

        public async Task<OperationResult> CalculateAmmountToPay(InOutParkingDTO parking)
        {
            

            var resultOperation = new OperationResult();

            if (string.IsNullOrEmpty(parking.TagNumber) || string.IsNullOrWhiteSpace(parking.TagNumber))
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "Tag Number can't be null";
                return resultOperation;
            }

            var parkingDB = await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
            var result = await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
            if (result == null || result.ExitTime != null)
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "The vehicle has not entered";
                return resultOperation;
            }
            
            TimeSpan totalHoursParked = result.EntryTime - DateTime.Now;
            resultOperation.IsValid = true;
            var toPay= AmmountToPay(result);
            resultOperation.Message = string.Format("Value to pay ${toPay}");
            return resultOperation;

        }
        private async Task<bool> IsVehicleParked(InOutParkingDTO parking)
        {
            var result = await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
            if (result == null || result.ExitTime != null)
            {
                return false;
            }
            return true;
        }
        public async Task<OperationResult> Update(InOutParkingDTO parking)
        {
            var resultOperation = new OperationResult();
            if (string.IsNullOrEmpty(parking.TagNumber) || string.IsNullOrWhiteSpace(parking.TagNumber))
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "Tag Number shoud have a value";
                return resultOperation;
            }
            
            var result = await _parkingRepository.GetLastParkingByTag(parking.TagNumber);
            if (result == null || result.ExitTime != null)
            {
                resultOperation.IsValid = false;
                resultOperation.Message = "The vehicle has not entered";
                return resultOperation;
            }
            
            var parkingAdd = mapper.Map<Parking>(parking);
            parkingAdd.Id = result.Id;
            parkingAdd.ExitTime = DateTime.Now;
            parkingAdd.EntryTime = result.EntryTime;
            var ammountToPay= AmmountToPay(parkingAdd);
            parkingAdd.FeePaid = ammountToPay;
            var success =await _parkingRepository.Update(parkingAdd.Id, parkingAdd);
            resultOperation.IsValid = success;
            resultOperation.Message = string.Format("Successful, Fee to pay is {0}", ammountToPay);
            return resultOperation;
        }
    }
}
