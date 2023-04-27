using AutoMapper;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Core.Entities;
using ParkingLotManagement.Domain.DTOs;
using ParkingLotManagement.Infrastructure.Repositories;
using System.Data.SqlClient;

namespace ParkingLotManagement.Application.Services
{
    public class ParkingServices : IParkingServices
    {
        private readonly IParkingRepository _parkingRepository;
        private readonly IMapper mapper;
        public ParkingServices(IParkingRepository parkingRepository, IMapper mapper)
        {
            _parkingRepository = parkingRepository;
            this.mapper = mapper;
        }

        public async Task<List<ParkedCarDTO>> GetAllAsync()
        {
         var parked=await _parkingRepository.GetAllAsync();
            return mapper.Map<List<ParkedCarDTO>>(parked);
        }

        public async Task<bool> Add(InOutParkingDTO parking)
        {
            var parkingAdd=mapper.Map<Parking>(parking);
            return await _parkingRepository.Add(parkingAdd);
        }

        public async Task<bool> Update(InOutParkingDTO parking)
        {
            var parkingAdd = mapper.Map<Parking>(parking);
            return await _parkingRepository.Update(parkingAdd.Id, parkingAdd);
        }
    }
}
