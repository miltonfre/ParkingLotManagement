
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace ParkingLotManagement.Application.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating database.");

                    logger.LogInformation("Creating DB if not exist.");

                    CreateDatabase(configuration);

                    var connection = new SqlConnection((configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
                    
                    connection.Open();
                    
                    

                    using var command = new SqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ParkingDB') CREATE DATABASE [ParkingDB];";
                    command.ExecuteNonQuery();

                    command.CommandText = "DROP TABLE IF EXISTS Parking";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Parking( Id int IDENTITY(1,1) NOT NULL, 
                                                                 TagNumber VARCHAR(10) NOT NULL,
                                                                 EntryTime datetime NOT NULL,
                                                                 ExitTime datetime NULL,
                                                                 CONSTRAINT PK_ParkingSpot PRIMARY KEY (Id))";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database.");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");
                }
            }
            return host;
        }

        private static void CreateDatabase(IConfiguration configuration)
        {


            var connection = new SqlConnection(configuration.GetValue<string>("DatabaseSettings:DefaulConnectionString"));
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"SELECT DB_ID('ParkingDB')";
            var result = command.ExecuteScalar();

            if (result == DBNull.Value)
            {
                
                    command.CommandText = "CREATE DATABASE ParkingDB";
                    command.ExecuteNonQuery();
            }

            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
            }
        }





    }
}
