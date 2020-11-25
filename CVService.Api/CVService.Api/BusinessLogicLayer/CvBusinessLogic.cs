using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using CVService.Api.BusinessLogicLayer.Abstracts;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.BusinessLogicLayer
{
    //This class acts as a facade at the moment so there are no unit tests for it, if in the future there is any additional logic added then
    //unit tests should be written to test the extra logic.
    public class CvBusinessLogic : BusinessLogicBase<Cv>,ICvBusinessLogic
    {
        private readonly ICvRepository _repository;

        public CvBusinessLogic(ICvRepository repository) : base(repository)
        {
            Guard.Against.Null(repository, nameof(repository));
            _repository = repository;
        }

        public async Task<IEnumerable<Skill>> GetSkills(int cvId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            return await _repository.GetSkills(cvId);
        }

        public async Task<IEnumerable<CompanyHistory>> GetCompanyHistories(int cvId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            return await _repository.GetCompanyHistories(cvId);
        }

        public async Task<Skill> GetSkill(int cvId, int skillId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(skillId, nameof(skillId));
            return await _repository.GetSkill(cvId, skillId);
        }

        public async Task<CompanyHistory> GetCompanyHistory(int cvId, int companyHistoryId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(companyHistoryId, nameof(companyHistoryId));
            return await _repository.GetCompanyHistory(cvId, companyHistoryId);
        }

        public async Task<Skill> AddSkill(int cvId, Skill skill)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(skill, nameof(skill));
            return await _repository.AddSkill(cvId, skill);
        }

        public async Task<CompanyHistory> AddCompanyHistory(int cvId, CompanyHistory companyHistory)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(companyHistory, nameof(companyHistory));
            return await _repository.AddCompanyHistory(cvId, companyHistory);
        }

        public async Task<Skill> EditSkill(int cvId, Skill skill)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(skill, nameof(skill));
            return await _repository.EditSkill(cvId, skill);
        }

        public async Task<CompanyHistory> EditCompanyHistory(int cvId, CompanyHistory companyHistory)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Null(companyHistory, nameof(companyHistory));
            return await _repository.EditCompanyHistory(cvId, companyHistory);
        }

        public async Task<Skill> RemoveSkill(int cvId, int skillId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(skillId, nameof(skillId));
            return await _repository.RemoveSkill(cvId, skillId);
        }

        public async Task<CompanyHistory> RemoveCompanyHistory(int cvId, int companyHistoryId)
        {
            Guard.Against.Default(cvId, nameof(cvId));
            Guard.Against.Default(companyHistoryId, nameof(companyHistoryId));
            return await _repository.RemoveCompanyHistory(cvId, companyHistoryId);
        }

        //TODO: Tech Test - An example XML method comment to show how I would document my code.
        /// <summary>
        /// Checks whether or not  an entity exists inside a cv
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="cvId"></param>
        /// <param name="entityId"></param>
        /// <returns>Returns boolean value for whether or not entity exists in cv</returns>
        public async Task<bool> DoesEntityExistInCvAsync<TEntity>(int cvId, int entityId) where TEntity : class, IHasId, IHasCvId
        {
            Guard.Against.Default(entityId, nameof(entityId));
            return await _repository.DoesEntityExistInCvAsync<TEntity>(cvId,entityId);
        }
    }
}