using System.Collections.Generic;
using System.Net;
using CVService.Api.WebLayer.Models;
using RestSharp;
using Shouldly;
using Xunit;

namespace CVService.Api.Tests.Integration
{
    [Collection("SerialTests")]
    public class AuthenticationTests : IntegrationTestBase
    {
        private readonly RestClient _client = new RestClient("https://localhost:44341/api/v1/cv");

        [Fact]
        public void Request_made_without_specifying_an_access_token_returns_http_unauthorised()
        {
            var request = new RestRequest();
            var response = _client.Get<IEnumerable<CvViewModel>>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void Request_made_with_specifying_an_access_token_in_url_returns_success()
        {
            var request = new RestRequest();
            request.AddQueryParameter("PrivateAccessToken", "84157CEC-965E-4680-BDD8-AFFD81AD0D2A");
            var response = _client.Get<IEnumerable<CvViewModel>>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void Request_made_with_specifying_an_access_token_in_header_returns_success()
        {
            var request = new RestRequest();
            request.AddHeader("PrivateAccessToken", "84157CEC-965E-4680-BDD8-AFFD81AD0D2A");
            var response = _client.Get<IEnumerable<CvViewModel>>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
