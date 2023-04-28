namespace ParkingLotManagement.Infrastructure.Repositories
{
    public interface IStatsRepository
    {
       Task<decimal> AverageRevenuePerDay();
        Task<decimal> AverageCarsPerDay();
        Task<decimal> TotalRevenueToday();
    }
}
