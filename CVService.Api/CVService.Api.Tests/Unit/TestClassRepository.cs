using CVService.Api.DataLayer;
using CVService.Api.DataLayer.Abstracts;

namespace CVService.Api.Tests.Unit
{
    public class TestClassRepository : RepositoryBase<TestClass>
    {
        public TestClassRepository(ApiContext context) : base(context)
        {
        }
    }
}