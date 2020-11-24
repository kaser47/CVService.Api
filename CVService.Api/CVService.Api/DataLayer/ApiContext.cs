using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CVService.Api.DataLayer
{
    public class ApiContext : DbContext, IApiContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Cv> Cvs { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<CompanyHistory> CompanyHistories { get; set; }

        //TODO: Tech Test - In a real-world app I would set up code coverage, and I would exclude code blocks like this using the attribute
        //This would be excluded from code coverage because its not doing much outside of EF Core
        [ExcludeFromCodeCoverage]
        public virtual void SetModifiedState(IHasId entity)
        {
            Guard.Against.Null(entity, nameof(entity));
            this.Entry(entity).State = EntityState.Modified;
        }
    }
}
