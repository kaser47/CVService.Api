using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.WebLayer
{
    public class CompanyHistoryBusinessLogic : BusinessLogic<CompanyHistory>
    {
        public CompanyHistoryBusinessLogic(IRepositoryBase<CompanyHistory> repository) : base(repository)
        {
        }
    }
}