using System.Collections.Generic;
using System.Threading.Tasks;

namespace CVService.Api.BusinessLogicLayer.Abstracts
{
    public interface ICrudBusinessLogic<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<bool> DoesExistAsync(int id);
    }
}
