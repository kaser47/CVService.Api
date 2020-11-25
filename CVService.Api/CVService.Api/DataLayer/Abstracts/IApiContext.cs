using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CVService.Api.DataLayer.Abstracts
{
    public interface IApiContext
    {
        DbSet<Cv> Cvs { get; set; }
        DbSet<Skill> Skills { get; set; }
        DbSet<CompanyHistory> CompanyHistories { get; set; }
        void SetModifiedState(IHasId entity);
    }
}