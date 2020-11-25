using CVService.Api.BusinessLogicLayer.Abstracts;
using CVService.Api.DataLayer.Abstracts;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.BusinessLogicLayer
{
    public class CompanyHistoryBusinessLogic : BusinessLogicBase<CompanyHistory>
    {
        public CompanyHistoryBusinessLogic(IRepositoryBase<CompanyHistory> repository) : base(repository)
        {
        }
    }
}