using Application.Contracts;
using Domain.Entities.Products;
using Persistance.ConcractsImplementation;
using Persistance.Contracts;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly SqlContext _context;
       // private readonly IDbConnection _connection;
        private bool _disposed;

        public UnitOfWork(SqlContext context)
        {
            _context = context;
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class => new Repository<SqlContext, TEntity>(_context);
        public async Task SaveChangesAsync()
        {
            
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

       
    }
}
