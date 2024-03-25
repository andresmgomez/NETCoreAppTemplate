using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TemplateRESTful.Persistence.Data.Contexts;
using TemplateRESTful.Persistence.Data.Interfaces;
using TemplateRESTful.Persistence.Storage.DbContexts;

namespace TemplateRESTful.Persistence.Data.Generic
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
