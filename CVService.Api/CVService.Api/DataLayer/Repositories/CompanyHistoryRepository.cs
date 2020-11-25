using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.DataLayer.Repositories
{
    public class CompanyHistoryRepository : RepositoryBase<CompanyHistory>
    {
        public CompanyHistoryRepository(ApiContext context) : base(context)
        {
        }
    }
}