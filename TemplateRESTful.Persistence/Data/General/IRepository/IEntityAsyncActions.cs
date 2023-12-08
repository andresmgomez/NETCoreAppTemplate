using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Persistence.Data
{
    public interface IEntityAsyncActions<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<T> GetEntityByIdAsync(int id);
        Task<List<T>> GetAllEntitiesAsync();
        Task<T> AddEntityAsync(T entity);
        Task UpdateEntity(T entity);
        Task DeleteEntity(T entity);
    }
}
