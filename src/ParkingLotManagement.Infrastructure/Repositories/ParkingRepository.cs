
using Microsoft.Extensions.Configuration;
using ParkingLotManagement.Core.Entities;
using System.Data.SqlClient;


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
            command.Parameters.AddWithValue("@EntryTime", DateTime.Now);
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
                                    "ExitTime=@ExitTime," +
                                    "FeePaid=@FeePaid " +
                                    "WHERE Id=@Id";

            command.Parameters.AddWithValue("@TagNumber", parking.TagNumber);
            command.Parameters.AddWithValue("@EntryTime", parking.EntryTime);
            command.Parameters.AddWithValue("@ExitTime", parking.ExitTime);
            command.Parameters.AddWithValue("@FeePaid", parking.FeePaid);
            command.Parameters.AddWithValue("@Id", parking.Id);
            var affected = await command.ExecuteNonQueryAsync();
            if (affected == 0)
                return false;

            return true;
        }
        public async Task<Parking> GetLastParkingByTag(string tagNumber)
        {
            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"SELECT TOP(1) Id, TagNumber ,EntryTime, ExitTime FROM Parking WHERE TagNumber=@TagNumber ORDER BY EntryTime DESC";
            command.Parameters.AddWithValue("@TagNumber", tagNumber);
            var result = await command.ExecuteReaderAsync();
            if (result.HasRows)
            {
                var parking = new Parking();
                while (result.Read())
                {
                    parking = FillParking(result);
                }
                return parking;
            }
            return null;

        }
        public async Task<List<Parking>> GetAllAsync()
        {

            var connection = new SqlConnection((_configuration["DatabaseSettings:ConnectionString"]));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"SELECT  Id, TagNumber ,EntryTime, ExitTime FROM Parking WHERE ExitTime IS NULL";


            var drlector = await command.ExecuteReaderAsync();
            var result = new List<Parking>();

            while (drlector.Read())
            {
                var parking = FillParking(drlector);
                result.Add(parking);
            }
            return result;

        }
        private Parking FillParking(SqlDataReader reader)
        {
            var parking = new Parking();
            parking.Id = int.Parse(reader["Id"].ToString().Trim());
            parking.TagNumber = reader["TagNumber"].ToString().Trim();
            parking.EntryTime = Convert.ToDateTime(reader["EntryTime"]);
            parking.ExitTime = reader.IsDBNull(reader.GetOrdinal("ExitTime")) ? null : Convert.ToDateTime(reader["ExitTime"]);
            return parking;
        }
    }
}
