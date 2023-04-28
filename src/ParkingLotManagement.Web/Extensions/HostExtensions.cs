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
                    string connString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
                    CreateDatabase(connString);

                    var connection = new SqlConnection(connString);

                    connection.Open();


                    using var command = new SqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ParkingDB') CREATE DATABASE [ParkingDB];";
                    command.ExecuteNonQuery();

                    command.CommandText = "DROP TABLE IF EXISTS Parking";
                    command.ExecuteNonQuery();

                    command.CommandText = @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Parking' and xtype='U')
                                                        CREATE TABLE Parking( Id int    IDENTITY(1,1) NOT NULL, 
                                                                         TagNumber  VARCHAR(10) NOT NULL,
                                                                         EntryTime  DATETIME NOT NULL,
                                                                         ExitTime   DATETIME NULL,
                                                                         FeePaid    DECIMAL(10, 2),
                                                                         CONSTRAINT PK_ParkingSpot PRIMARY KEY (Id))";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Creating procedures");
                    CreateProcedures(connString);
                    logger.LogInformation("Procedures created");
                    logger.LogInformation("Database Migration endedn succesful");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");
                }
            }
            return host;
        }
        private static void CreateProcedures(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand
            {
                Connection = connection
            };

            command.CommandText = @"CREATE OR ALTER PROCEDURE AverageCarsPerDay
                                            AS
                                            SELECT AVG(CarsPerDay) AS AverageCarsPerDay
                                            FROM (
                                                SELECT CONVERT(DATE, EntryTime) AS EntryDate, COUNT(*) AS CarsPerDay
                                                FROM Parking
                                                WHERE EntryTime >= DATEADD(day, -30, GETDATE())
                                                GROUP BY CONVERT(DATE, EntryTime)
                                            ) AS DailyCars";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE OR ALTER PROCEDURE AverageRevenuePerDay
                                    AS
                                    SELECT AVG(RevenuePerDay) AS AverageRevenuePerDay
                                    FROM (
                                        SELECT CONVERT(DATE, EntryTime) AS EntryDate, SUM(FeePaid) AS RevenuePerDay
                                        FROM Parking
                                        WHERE EntryTime >= DATEADD(day, -30, GETDATE()) AND ExitTime IS NOT NULL
                                        GROUP BY CONVERT(DATE, EntryTime)
                                    ) AS DailyRevenue";
            command.ExecuteNonQuery();

            command.CommandText = @"CREATE OR ALTER PROCEDURE TotalRevenueToday 
                                    AS
                                        SELECT SUM(FeePaid) as TotalRevenueToday 
                                        FROM Parking
                                        WHERE ExitTime >= CONVERT(date, GETDATE())";
            command.ExecuteNonQuery();


        }
        private static void CreateDatabase(string connectionString)
        {


            var connection = new SqlConnection(connectionString);
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
