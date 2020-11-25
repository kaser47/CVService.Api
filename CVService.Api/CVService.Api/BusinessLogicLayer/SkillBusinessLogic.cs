using CVService.Api.BusinessLogicLayer.Abstracts;
using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.BusinessLogicLayer
{
    public class SkillBusinessLogic : BusinessLogicBase<Skill>
    {
        public SkillBusinessLogic(IRepositoryBase<Skill> repository) : base(repository)
        {
        }
    }
}