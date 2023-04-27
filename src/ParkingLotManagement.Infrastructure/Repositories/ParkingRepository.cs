
using Microsoft.Extensions.Configuration;
using ParkingLotManagement.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Transactions;

namespace ParkingLotManagement.Infrastructure.Repositories
{
   
    public class ParkingRepository : IParkingRepository
    {
        private readonly IConfiguration _configuration;
        public ParkingRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Add(Parking parking)
        {
           var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = "INSERT INTO Parking (TagNumber,EntryTime) Values (@TagNumber,@EntryTime)";
            command.Parameters.AddWithValue("@TagNumber", parking.TagNumber);
            command.Parameters.AddWithValue("@EntryTime", DateTime.UtcNow);
            var affected = await command.ExecuteNonQueryAsync();            
            if (affected == 0)
                return false;

            return true;          
        }

        public async Task<bool> Update(int id, Parking parking)
        {
            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"UPDATE Parking SET " +
                                    "TagNumber=@TagNumber" +
                                    ",EntryTime=@EntryTime," +
                                    "ExitTime=@ExitTime" +
                                    "WHERE Id=@Id";

            command.Parameters.AddWithValue("@TagNumber", parking.TagNumber);
            command.Parameters.AddWithValue("@EntryTime", parking.EntryTime);
            command.Parameters.AddWithValue("@ExitTime", parking.ExitTime);
            command.Parameters.AddWithValue("@Id", parking.ExitTime);
            var affected = await command.ExecuteNonQueryAsync();
            if (affected == 0)
                return false;

            return true;
        }

        public async Task<List<Parking>> GetAllAsync()
        {

            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"SELECT  TagNumber ,EntryTime, ExitTime FROM Parking WHERE ExitTime IS NULL";


            var drlector = await command.ExecuteReaderAsync();
            var result = new List<Parking>();

            while (drlector.Read())
            {
                var parking = new Parking();
                parking.TagNumber = drlector["TagNumber"].ToString().Trim();
                parking.EntryTime = Convert.ToDateTime(drlector["EntryTime"]) ;
                parking.ExitTime = drlector.IsDBNull(drlector.GetOrdinal("ExitTime")) ? null : Convert.ToDateTime(drlector["ExitTime"]);
                result.Add(parking);
            }
            return result;

        }
    }
}
