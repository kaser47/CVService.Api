using CVService.Api.DataLayer.Models;

namespace CVService.Api.DataLayer.Abstracts
{
    public class SkillRepository : RepositoryBase<Skill>
    {
        public SkillRepository(ApiContext context) : base(context)
        {
        }
    }
}