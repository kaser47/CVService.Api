using CVService.Api.DataLayer;
using CVService.Api.DataLayer.Abstracts;

namespace CVService.Api.Tests.Unit
{
    public class TestClassRepository : RepositoryBase<HasIdTestDouble>
    {
        public TestClassRepository(ApiContext context) : base(context)
        {
        }
    }
}