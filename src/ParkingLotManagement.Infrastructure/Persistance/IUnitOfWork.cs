using ParkingLotManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotManagement.Infrastructure.Persistance
{
    public interface IUnitOfWork : IDisposable
    {
        IParkingRepository ParkingRepository { get; }

        void Commit();
    }
}
