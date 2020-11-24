using System.Collections.Generic;
using System.Threading.Tasks;
using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.DataLayer.Abstracts
{
    public interface IRepositoryBase<T> where T : IHasId
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> DoesExistAsync(int id);
    }
}