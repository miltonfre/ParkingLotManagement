using AutoMapper;
using ParkingLotManagement.Application.DTOs;
using ParkingLotManagement.Core.Entities;
using ParkingLotManagement.Domain.DTOs;

namespace ParkingLotManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Parking,ParkedCarDTO >().ReverseMap();
            CreateMap<Parking, InOutParkingDTO>().ReverseMap();
        }
    }
}
