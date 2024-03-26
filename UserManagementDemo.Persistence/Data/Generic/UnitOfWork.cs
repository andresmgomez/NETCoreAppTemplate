using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UserManagementDemo.Persistence.Data.Contexts;
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Persistence.Storage.DbContexts;

namespace UserManagementDemo.Persistence.Data.Generic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAccessUserContext _userContext;
        private readonly ApplicationDbContext _dbContext;
        private bool disposed;

        public UnitOfWork(ApplicationDbContext dbContext, IAccessUserContext userContext)
        {
            _dbContext = dbContext;
            _userContext = userContext;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(_userContext.UserId);
        }

        public Task RollBack()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed) 
            {
                if (disposing) 
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }
    }
}
