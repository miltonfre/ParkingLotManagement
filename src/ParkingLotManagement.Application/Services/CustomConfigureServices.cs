using Microsoft.Extensions.Configuration;
using ParkingLotManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotManagement.Application.Services
{
    public class CustomConfigureServices : ICustomConfigureServices
    {
       private readonly IConfiguration _configuration;

        public CustomConfigureServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int CapacitySpots()
        {
            return int.Parse(_configuration["CustomSettings:CapacitySpots"]);
        }

        public decimal HourlyFee()
        {
            return decimal.Parse(_configuration["CustomSettings:HourlyFee"]);
        }
    }
}
