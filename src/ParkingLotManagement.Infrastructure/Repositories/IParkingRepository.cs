using ParkingLotManagement.Core.Entities;

namespace ParkingLotManagement.Infrastructure.Repositories
{
    public interface IParkingRepository
    {
        Task<bool> Add(Parking parking);
        Task<bool> Update(int id, Parking parking);
        Task<List<Parking>> GetAllAsync();
        Task<Parking> GetLastParkingByTag(string tagNumber);
    }
}
