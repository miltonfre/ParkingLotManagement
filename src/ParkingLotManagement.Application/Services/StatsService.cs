using ParkingLotManagement.Application.Interfaces;
using ParkingLotManagement.Infrastructure.Repositories;


namespace ParkingLotManagement.Application.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatsRepository _statsRepository;

        public StatsService(IStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }
        public async Task<decimal> AverageCarsPerDay()
        {
            return await _statsRepository.AverageCarsPerDay();
        }

        public async Task<decimal> AverageRevenuePerDay()
        {
            return await _statsRepository.AverageRevenuePerDay();
        }
    }
}
