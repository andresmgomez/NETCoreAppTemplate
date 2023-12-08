using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Persistence.Storage.DbContexts;

namespace TemplateRESTful.Persistence.Data
{
    public class EntityAsyncActions<T> : IEntityAsyncActions<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public EntityAsyncActions(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> GetEntityByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllEntitiesAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddEntityAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateEntity(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteEntity(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }
    }
}
