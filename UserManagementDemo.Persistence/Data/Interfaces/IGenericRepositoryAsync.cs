using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Persistence.Data.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
