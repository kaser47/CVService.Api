using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Abstracts;

namespace CVService.Api.BusinessLogicLayer.Abstracts
{
    //This class acts as a facade at the moment so there are no unit tests for it, if in the future there is any additional logic added then
    //unit tests should be written to test the extra logic.
    public abstract class BusinessLogicBase<T> : ICrudBusinessLogic<T> where T : class, IHasId
    {
        private readonly IRepositoryBase<T> _repository;

        protected BusinessLogicBase(IRepositoryBase<T> repository)
        {
            Guard.Against.Null(repository, nameof(repository));
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            Guard.Against.Default(id, nameof(id));
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            return await _repository.AddAsync(entity);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            return await _repository.UpdateAsync(entity);
        }

        public async Task<T> RemoveAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.ReadAllAsync();
        }

        public async Task<bool> DoesExistAsync(int id)
        {
            Guard.Against.Default(id, nameof(id));
            return await _repository.DoesExistAsync(id);
        }
    }
}