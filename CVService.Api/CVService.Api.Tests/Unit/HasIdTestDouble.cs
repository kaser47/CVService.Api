using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.Tests.Unit
{
    public class HasIdTestDouble : IHasId
    {
        public int Id { get; set; }
    }
}