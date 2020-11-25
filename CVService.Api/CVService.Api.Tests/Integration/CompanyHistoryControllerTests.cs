using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CVService.Api.CommonLayer;
using CVService.Api.WebLayer.Models;
using RestSharp;
using Shouldly;
using Xunit;

namespace CVService.Api.Tests.Integration
{
    [Collection("SerialTests")]
    public class CompanyHistoryControllerTests : IntegrationTestBase
    {

        [Fact]
        public void CompanyHistoryController_Get_Returns_SeedData()
        {
            var request = MakeRequest("company-histories");
            var response = Client.Get<IEnumerable<CompanyHistoryViewModel>>(request);

            response.Data.Count().ShouldBe(6);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CompanyHistoryController_GetById_Returns_CorrectSeedData()
        {
            var request = MakeRequest("company-histories/1");
            var response = Client.Get<CompanyHistoryViewModel>(request);

            response.Data.Id.ShouldBe(SeedData.SeedCompanyHistories[0].Id);
            response.Data.Name.ShouldBe(SeedData.SeedCompanyHistories[0].Name);
            response.Data.CvId.ShouldBe(SeedData.SeedCompanyHistories[0].CvId);
            response.Data.Description.ShouldBe(SeedData.SeedCompanyHistories[0].Description);
            response.Data.StartDate.ShouldBe(SeedData.SeedCompanyHistories[0].StartDate);
            response.Data.EndDate.ShouldBe(SeedData.SeedCompanyHistories[0].EndDate);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CompanyHistoryController_GetByIncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("company-histories/7");
            var response = Client.Get<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        private CompanyHistoryViewModel GetCompanyHistory()
        {
            var request = MakeRequest("company-histories/1");
            var response = Client.Get<CompanyHistoryViewModel>(request);
            return response.Data;
        }

        [Fact]
        public void CompanyHistoryController_Put_DifferentIds_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories/1");
            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();

            companyHistoryViewModelToTest.Id++;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Put<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Put_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("company-histories/7");
            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();
            companyHistoryViewModelToTest.Id = 7;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Put<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CompanyHistoryController_Put_SameIds_Returns_Ok()
        {
            var request = MakeRequest("company-histories/1");
            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();
            string updatedName = companyHistoryViewModelToTest.Name + "Updated";
            companyHistoryViewModelToTest.Name = updatedName;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Put<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            CompanyHistoryViewModel updatedCompanyHistoryViewModel = GetCompanyHistory();
            updatedCompanyHistoryViewModel.Name.ShouldBe(updatedName);
        }

        [Fact]
        public void CompanyHistoryController_Post_NullCv_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = null;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_CvHasId_Returns_InternalServerError()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void CompanyHistoryController_Post_Valid_Returns_Created()
        {
            int initialCount = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("company-histories")).Data.Count();

            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Test Company History",
                CvId = 1,
                Description = "Test Description",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var results = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("company-histories"));
            results.Data.Count().ShouldBe(initialCount +1);
        }

        [Fact]
        public void CompanyHistoryController_Post_InvalidName_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Lorem ipsum dolor sit amet, consectetuer adipiscing",
                CvId = 1,
                Description = "Test Description",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_NoName_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                CvId = 1,
                Description = "Test Description",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_NoCvId_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Test Company History",
                Description = "Test Description",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_InvalidDescription_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Test Company History",
                CvId = 1,
                Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean " +
                              "commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus " +
                              "et magnis dis parturient montes, nascetur ridiculus mus. Donec quam " +
                              "felis, ultricies nec, pellentesque eu, pretium quis,.",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_NoStartDate_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Test Company History",
                CvId = 1,
                Description = "Test Description",
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Post_EndDateBeforeStartDate_Returns_BadRequest()
        {
            var request = MakeRequest("company-histories");
            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                Name = "Test Company History",
                CvId = 1,
                Description = "Test Description",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = DateTime.Now.AddYears(-2)
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CompanyHistoryController_Delete_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("company-histories/7");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CompanyHistoryController_Delete_CorrectId_Returns_Ok()
        {
            int initialCount = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("company-histories")).Data.Count();

            var request = MakeRequest("company-histories/1");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var results = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("company-histories"));
            results.Data.Count().ShouldBe(initialCount - 1);
        }
    }
}
