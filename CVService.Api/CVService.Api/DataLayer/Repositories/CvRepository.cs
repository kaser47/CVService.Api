using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CVService.Api.DataLayer.Repositories
{
    //TODO: Tech Test - If this was for production I would spend some time trying to make the repository designed for testability as
    //we cannot mock extension methods easily
    public class CvRepository : RepositoryBase<Cv>, ICvRepository
    {
        public CvRepository(ApiContext context) : base(context)
        {
            Guard.Against.Null(context, nameof(context));
        }

        public override async Task<Cv> GetByIdAsync(int id)
        {
            Guard.Against.Default(id, nameof(id));
            return await Context.Cvs
                .Include(x => x.Skills)
                .Include(x => x.CompanyHistories)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Cv>> ReadAllAsync()
        {
            return await this.Context.Cvs.Include((x) => x.Skills).Include(x => x.CompanyHistories).ToListAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkills(int cvId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            return await Context.Cvs.Where(x => x.Id == cvId).SelectMany(x => x.Skills).ToListAsync();
        }

        public async Task<IEnumerable<CompanyHistory>> GetCompanyHistories(int cvId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            return await Context.Cvs.Where(x => x.Id == cvId).SelectMany(x => x.CompanyHistories).ToListAsync();
        }

        public async Task<Skill> GetSkill(int cvId, int skillId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(skillId, nameof(skillId));

            var query = Context.Cvs
                .Where(x => x.Id == cvId)
                .Select(x => x.Skills.SingleOrDefault(s => s.Id == skillId)).AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<CompanyHistory> GetCompanyHistory(int cvId, int companyHistoryId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(companyHistoryId, nameof(companyHistoryId));

            var query = Context.Cvs
                .Where(x => x.Id == cvId)
                .Select(x => x.CompanyHistories.SingleOrDefault(s => s.Id == companyHistoryId)).AsQueryable();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Skill> AddSkill(int cvId, Skill skill)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(skill, nameof(skill));

            skill.CvId = cvId;
            await Context.Skills.AddAsync(skill);
            await Context.SaveChangesAsync();
            return skill;
        }

        public async Task<CompanyHistory> AddCompanyHistory(int cvId, CompanyHistory companyHistory)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(companyHistory, nameof(companyHistory));

            companyHistory.CvId = cvId;
            await Context.CompanyHistories.AddAsync(companyHistory);
            await Context.SaveChangesAsync();
            return companyHistory;
        }

        public async Task<Skill> EditSkill(int cvId, Skill skill)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(skill, nameof(skill));

            Context.SetModifiedState(skill);
            await Context.SaveChangesAsync();
            return skill;
        }

        public async Task<CompanyHistory> EditCompanyHistory(int cvId, CompanyHistory companyHistory)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(companyHistory, nameof(companyHistory));

            Context.SetModifiedState(companyHistory);
            await Context.SaveChangesAsync();
            return companyHistory;
        }

        public async Task<Skill> RemoveSkill(int cvId, int skillId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(skillId, nameof(skillId));

            var queryable = Context.Cvs.Where(x => x.Id == cvId)
                .Select(x => x.Skills.FirstOrDefault(s => s.Id == skillId)).AsQueryable();

            var entity = await queryable.FirstOrDefaultAsync();

            Context.Skills.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task<CompanyHistory> RemoveCompanyHistory(int cvId, int companyHistoryId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(companyHistoryId, nameof(companyHistoryId));

            var queryable = Context.Cvs.Where(x => x.Id == cvId)
                .Select(x => x.CompanyHistories.FirstOrDefault(c => c.Id == companyHistoryId)).AsQueryable();

            var entity = await queryable.FirstOrDefaultAsync();

            Context.CompanyHistories.Remove(entity);
            await Context.SaveChangesAsync();
            return entity;

        }

        //TODO: Tech Test - Entity can be any navigation property in this case skill or company history
        public async Task<bool> DoesEntityExistInCvAsync<TEntity>(int cvId, int entityId) where TEntity : class, IHasId, IHasCvId
        {
            Guard.Against.Default(entityId, nameof(entityId));
            return await Context.Set<TEntity>().AnyAsync(x => x.Id == entityId && x.CvId == cvId);
        }
    }
}