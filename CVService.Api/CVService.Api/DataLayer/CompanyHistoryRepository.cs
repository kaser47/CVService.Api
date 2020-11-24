using CVService.Api.DataLayer.Models;

namespace CVService.Api.DataLayer.Abstracts
{
    public class CompanyHistoryRepository : RepositoryBase<CompanyHistory>
    {
        public CompanyHistoryRepository(ApiContext context) : base(context)
        {
        }
    }
}