

using Microsoft.Extensions.Configuration;
using ParkingLotManagement.Infrastructure.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace ParkingLotManagement.Infrastructure.Persistance
{

    // more information https://github.com/timschreiber/DapperUnitOfWork/blob/master/DapperUnitOfWork/UnitOfWork.cs
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IParkingRepository _parkingRepository;
        private bool _disposed;
        private readonly IConfiguration _configuration;
        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connection = new SqlConnection(_configuration["DatabaseSettings:ConnectionString"]);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IParkingRepository ParkingRepository
        {
            get { return _parkingRepository; }//?? (_parkingRepository = new ParkingRepository(_transaction)); }
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            _parkingRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }

    }
}
