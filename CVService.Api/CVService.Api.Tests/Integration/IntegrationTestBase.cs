using System;
using System.Threading;
using RestSharp;

namespace CVService.Api.Tests.Integration
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly RestClient Client = new RestClient("https://localhost:44341/api/v1/");

        public RestRequest MakeRequest(string path) {
            RestRequest request = new RestRequest(path);
            request.AddHeader("PrivateAccessToken", "84157CEC-965E-4680-BDD8-AFFD81AD0D2A");
            return request;
        }
    
        public IntegrationTestBase()
        {
                var client = new RestClient("https://localhost:44341/Maintenance");
                var request = new RestRequest("CreateData");
                client.Get(request);
        }

        public void Dispose()
        {
            var client = new RestClient("https://localhost:44341/Maintenance");
            var request = new RestRequest("WipeData");
            client.Get(request);
        }
    }
}
