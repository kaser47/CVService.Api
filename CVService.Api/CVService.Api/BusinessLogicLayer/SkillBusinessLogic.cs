using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.WebLayer
{
    public class SkillBusinessLogic : BusinessLogic<Skill>
    {
        public SkillBusinessLogic(IRepositoryBase<Skill> repository) : base(repository)
        {
        }
    }
}