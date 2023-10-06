using Persistance.Contracts;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUnitOfWork:IDisposable
    {
        IRepository<TContext> GetRepository<TContext>() where TContext : class;
        Task SaveChangesAsync();

        void SaveChanges();
    }
}
