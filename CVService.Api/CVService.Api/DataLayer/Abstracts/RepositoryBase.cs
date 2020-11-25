using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.CommonLayer.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace CVService.Api.DataLayer.Abstracts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IHasId
    {
        public ApiContext Context { get; set; }

        protected RepositoryBase(ApiContext context)
        {
            Guard.Against.Null(context, nameof(context));
            this.Context = context;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            Guard.Against.Default(id, nameof(id));
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            var result = await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            Context.SetModifiedState(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await this.Context.Set<T>().ToListAsync();
        }


        public async Task<bool> DoesExistAsync(int id)
        {
            Guard.Against.Default(id, nameof(id));
            return await Context.Set<T>().AnyAsync(x => x.Id == id);
        }
    }
}