

using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ParkingLotManagement.Infrastructure.Repositories
{
    public class StatsRepository : IStatsRepository
    {
        private readonly IConfiguration _configuration;

        public StatsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<decimal> AverageCarsPerDay()
        {
            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AverageCarsPerDay";
          return  (decimal) await command.ExecuteScalarAsync();
        }

        public async Task<decimal> AverageRevenuePerDay()
        {
            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "AverageRevenuePerDay";
            return (decimal)await command.ExecuteScalarAsync();
        }
    }
}
