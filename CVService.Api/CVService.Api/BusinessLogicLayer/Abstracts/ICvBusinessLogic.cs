using System.Collections.Generic;
using System.Threading.Tasks;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.WebLayer.Abstracts
{
    public interface ICvBusinessLogic : ICrudBusinessLogic<Cv>
    {
        Task<IEnumerable<Skill>> GetSkills(int cvId);
        Task<IEnumerable<CompanyHistory>> GetCompanyHistories(int cvId);
        Task<Skill> GetSkill(int cvId, int skillId);
        Task<CompanyHistory> GetCompanyHistory(int cvId, int companyHistoryId);
        Task<Skill> AddSkill(int cvId, Skill skill);
        Task<CompanyHistory> AddCompanyHistory(int cvId, CompanyHistory companyHistory);
        Task<Skill> EditSkill(int cvId, Skill skill);
        Task<CompanyHistory> EditCompanyHistory(int cvId, CompanyHistory companyHistory);
        Task<Skill> RemoveSkill(int cvId, int skillId);
        Task<CompanyHistory> RemoveCompanyHistory(int cvId, int companyHistoryId);
        Task<bool> DoesEntityExistInCvAsync<TEntity>(int cvId,int entityId) where TEntity : class, IHasId, IHasCvId;
    }
}