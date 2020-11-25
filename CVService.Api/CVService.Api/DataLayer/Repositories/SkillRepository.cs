using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.DataLayer.Repositories
{
    public class SkillRepository : RepositoryBase<Skill>
    {
        public SkillRepository(ApiContext context) : base(context)
        {
        }
    }
}