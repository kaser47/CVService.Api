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
    [Collection("Integration")]
    public class CvControllerIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public void CVController_Get_Returns_SeedData()
        {
            var request = MakeRequest("cv");
            var response = Client.Get<IEnumerable<CvViewModel>>(request);

            response.Data.Count().ShouldBe(3);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetById_Returns_CorrectSeedData()
        {
            var request = MakeRequest("cv/1");
            var response = Client.Get<CvViewModel>(request);

            response.Data.Id.ShouldBe(SeedData.SeedCvs[0].Id);
            response.Data.Name.ShouldBe(SeedData.SeedCvs[0].Name);
            response.Data.EmailAddress.ShouldBe(SeedData.SeedCvs[0].EmailAddress);
            response.Data.CompanyHistories.Count().ShouldBe(3);
            response.Data.Skills.Count().ShouldBe(3);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetByIncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4");
            var response = Client.Get<CvViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        private CvViewModel GetCv()
        {
            var request = MakeRequest("cv/1");
            var response = Client.Get<CvViewModel>(request);
            return response.Data;
        }

        [Fact]
        public void CVController_Put_DifferentIds_Returns_BadRequest()
        {
            var request = MakeRequest("cv/1");
            CvViewModel cvViewModelToTest = GetCv();

            cvViewModelToTest.Id++;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Put<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_Put_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4");
            CvViewModel cvViewModelToTest = GetCv();
            cvViewModelToTest.Id = 4;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Put<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_Put_SameIds_Returns_Ok()
        {
            var request = MakeRequest("cv/1");
            CvViewModel cvViewModelToTest = GetCv();
            string updatedName = cvViewModelToTest.Name + "Updated";
            cvViewModelToTest.Name = updatedName;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Put<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            CvViewModel updatedCvViewModel = GetCv();
            updatedCvViewModel.Name.ShouldBe(updatedName);
        }

        [Fact]
        public void CVController_Post_NullCv_Returns_BadRequest()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = null;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_Post_CvHasId_Returns_InternalServerError()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = GetCv();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void CVController_Post_Valid_Returns_Created()
        {
            int initialCount = Client.Get<IEnumerable<CvViewModel>>(MakeRequest("cv")).Data.Count();

            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = new CvViewModel()
            {
                Name = "Test Cv",
                EmailAddress = "test@hotmail.com",
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var results = Client.Get<IEnumerable<CvViewModel>>(MakeRequest("cv"));
            results.Data.Count().ShouldBe(initialCount +1);
        }

        [Fact]
        public void CVController_Post_InvalidName_Returns_BadRequest()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = new CvViewModel()
            {
                Name = "Lorem ipsum dolor sit amet, consectetuer adipiscing",
                EmailAddress = "test@hotmail.com",
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        [Fact]
        public void CVController_Post_InvalidEmail_Returns_BadRequest()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = new CvViewModel()
            {
                Name = "Test Cv",
                EmailAddress = "testhotmail.com",
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_Post_DuplicateSkills_Returns_BadRequest()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = new CvViewModel()
            {
                Name = "Test Cv",
                EmailAddress = "test@hotmail.com",
                Skills = new List<SkillViewModel>()
                {
                    new SkillViewModel()
                    {
                        Name = "C#",
                        CvId = 4,
                        MonthsExperience = 20
                    },
                    new SkillViewModel()
                    {
                        Name = "C#",
                        CvId = 4,
                        MonthsExperience = 30
                    }
                }
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_Post_MoreThanOneCurrentJob_Returns_BadRequest()
        {
            var request = MakeRequest("cv");
            CvViewModel cvViewModelToTest = new CvViewModel()
            {
                Name = "Test Cv",
                EmailAddress = "test@hotmail.com",
                CompanyHistories = new List<CompanyHistoryViewModel>()
                {
                    new CompanyHistoryViewModel()
                    {
                        Name = "Google",
                        StartDate = DateTime.Now.AddYears(-1),
                        EndDate = null
                    },
                    new CompanyHistoryViewModel()
                    {
                        Name = "Facebook",
                        StartDate = DateTime.Now.AddYears(-3),
                        EndDate = null
                    },
                }
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(cvViewModelToTest);

            var response = Client.Post<CvViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_Delete_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_Delete_CorrectId_Returns_Ok()
        {
            int initialCount = Client.Get<IEnumerable<CvViewModel>>(MakeRequest("cv")).Data.Count();

            var request = MakeRequest("cv/1");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var results = Client.Get<IEnumerable<CvViewModel>>(MakeRequest("cv"));
            results.Data.Count().ShouldBe(initialCount - 1);
        }

        [Fact]
        public void CVController_GetSkills_CorrectId_Returns_SkillData()
        {
            var request = MakeRequest("cv/1/skills");
            var response = Client.Get<IEnumerable<SkillViewModel>>(request);

            response.Data.Count().ShouldBe(3);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetSkills_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/skills");
            var response = Client.Get<IEnumerable<SkillViewModel>>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_GetCompanyHistories_CorrectId_Returns_CompanyHistoryData()
        {
            var request = MakeRequest("cv/1/company-histories");
            var response = Client.Get<IEnumerable<CompanyHistoryViewModel>>(request);

            response.Data.Count().ShouldBe(3);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetCompanyHistories_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/company-histories");
            var response = Client.Get<IEnumerable<CompanyHistoryViewModel>>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_GetSkill_CorrectId_Returns_SkillData()
        {
            var request = MakeRequest("cv/1/skills/1");
            var response = Client.Get<SkillViewModel>(request);

            response.Data.Name.ShouldBe(SeedData.SeedSkills[0].Name);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetSkill_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/skills/4");
            var response = Client.Get<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_GetCompanyHistory_CorrectId_Returns_CompanyHistoryData()
        {
            var request = MakeRequest("cv/1/company-histories/1");
            var response = Client.Get<CompanyHistoryViewModel>(request);

            response.Data.Name.ShouldBe(SeedData.SeedCompanyHistories[0].Name);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void CVController_GetCompanyHistory_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/company-histories/4");
            var response = Client.Get<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        [Fact]
        public void CVController_PostSkill_NullSkill_Returns_BadRequest()
        {
            var request = MakeRequest("cv/1/skills");

            SkillViewModel skillViewModelToTest = null;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        private SkillViewModel GetSkill()
        {
            var request = MakeRequest("cv/1/skills/1");
            return Client.Get<SkillViewModel>(request).Data;
        }

        [Fact]
        public void CVController_PostSkill_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/skills");

            SkillViewModel skillViewModelToTest = GetSkill();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Post<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PostSkill_SkillIncludesId_Returns_InternalServerError()
        {
            var request = MakeRequest("cv/1/skills");

            SkillViewModel skillViewModelToTest = GetSkill();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Post<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void CVController_PostSkill_Valid_Returns_Created()
        {
            int initialCount = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("cv/1/skills")).Data.Count();

            var request = MakeRequest("cv/1/skills");

            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                CvId = 1,
                Name = "XUnit",
                MonthsExperience = 10
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Post<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var results = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("cv/1/skills"));
            results.Data.Count().ShouldBe(initialCount +1);
        }

        [Fact]
        public void CVController_PostCompanyHistory_NullCompanyHistory_Returns_BadRequest()
        {
            var request = MakeRequest("cv/1/company-histories");

            CompanyHistoryViewModel companyHistoryViewModelToTest = null;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);

            var response = Client.Post<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        private CompanyHistoryViewModel GetCompanyHistory()
        {
            var request = MakeRequest("cv/1/company-histories/1");
            return Client.Get<CompanyHistoryViewModel>(request).Data;
        }

        [Fact]
        public void CVController_PostCompanyHistory_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/company-histories");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Post<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PostCompanyHistory_IncludesId_Returns_InternalServerError()
        {
            var request = MakeRequest("cv/1/company-histories");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Post<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void CVController_PostCompanyHistory_Valid_Returns_Created()
        {
            int initialCount = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("cv/1/company-histories")).Data.Count();

            var request = MakeRequest("cv/1/company-histories");

            CompanyHistoryViewModel companyHistoryViewModelToTest = new CompanyHistoryViewModel()
            {
                CvId = 1,
                Name = "Google",
                Description = "I worked at google",
                StartDate = DateTime.Now.AddYears(-1),
                EndDate = null
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Post<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var results = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("cv/1/company-histories"));
            results.Data.Count().ShouldBe(initialCount + 1);
        }

        [Fact]
        public void CVController_PutSkill_DifferentIds_Returns_BadRequest()
        {
            var request = MakeRequest("cv/1/skills/1");

            SkillViewModel skillViewModelToTest = GetSkill();
            skillViewModelToTest.Id++;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Put<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_PutSkill_IncorrectCvId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/skills/1");

            SkillViewModel skillViewModelToTest = GetSkill();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Put<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PutSkill_IncorrectSkillId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/skills/4");

            SkillViewModel skillViewModelToTest = GetSkill();
            skillViewModelToTest.Id = 4;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Put<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PutSkill_Valid_Returns_Ok()
        {
            var request = MakeRequest("cv/1/skills/1");

            SkillViewModel skillViewModelToTest = GetSkill();
            string updatedName = skillViewModelToTest.Name + "Updated";
            skillViewModelToTest.Name = updatedName;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);
            var response = Client.Put<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            SkillViewModel updatedSkillViewModel = GetSkill();
            updatedSkillViewModel.Name.ShouldBe(updatedName);
        }

        [Fact]
        public void CVController_PutCompanyHistory_DifferentIds_Returns_BadRequest()
        {
            var request = MakeRequest("cv/1/company-histories/1");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();
            companyHistoryViewModelToTest.Id++;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Put<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void CVController_PutCompanyHistory_IncorrectCvId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/company-histories/1");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Put<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PutCompanyHistory_IncorrectSkillId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/company-histories/4");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();
            companyHistoryViewModelToTest.Id = 4;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Put<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_PutCompanyHistory_Valid_Returns_Ok()
        {
            var request = MakeRequest("cv/1/company-histories/1");

            CompanyHistoryViewModel companyHistoryViewModelToTest = GetCompanyHistory();
            string updatedName = companyHistoryViewModelToTest.Name + "Updated";
            companyHistoryViewModelToTest.Name = updatedName;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(companyHistoryViewModelToTest);
            var response = Client.Put<CompanyHistoryViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            CompanyHistoryViewModel updatedCompanyHistoryViewModel = GetCompanyHistory();
            updatedCompanyHistoryViewModel.Name = updatedName;
        }


        [Fact]
        public void CVController_DeleteSkill_IncorrectCvId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/skills/1");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_DeleteSkill_IncorrectSkillId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/skills/4");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_DeleteSkill_Valid_Returns_Ok()
        {
            int initialCount = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("cv/1/skills")).Data.Count();

            var request = MakeRequest("cv/1/skills/1");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var results = Client.Get<IEnumerable<CvViewModel>>(MakeRequest("cv/1/skills"));
            results.Data.Count().ShouldBe(initialCount - 1);
        }

        [Fact]
        public void CVController_DeleteCompanyHistory_IncorrectCvId_Returns_NotFound()
        {
            var request = MakeRequest("cv/4/company-histories/1");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_DeleteCompanyHistory_IncorrectCompanyHistoryId_Returns_NotFound()
        {
            var request = MakeRequest("cv/1/company-histories/4");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void CVController_DeleteCompanyHistory_Valid_Returns_Ok()
        {
            int initialCount = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("cv/1/company-histories")).Data.Count();

            var request = MakeRequest("cv/1/company-histories/1");
            var response = Client.Delete(request);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var results = Client.Get<IEnumerable<CompanyHistoryViewModel>>(MakeRequest("cv/1/company-histories"));
            results.Data.Count().ShouldBe(initialCount - 1);
        }
    }
}