namespace ParkingLotManagement.Application.Interfaces
{
    public interface IStatsService
    {
        Task<decimal> AverageRevenuePerDay();
        Task<decimal> AverageCarsPerDay();
        Task<decimal> TotalRevenueToday();
    }
}
