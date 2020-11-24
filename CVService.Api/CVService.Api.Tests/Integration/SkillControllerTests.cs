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
    public class SkillControllerTests : IntegrationTestBase
    {
        [Fact]
        public void SkillController_Get_Returns_SeedData()
        {
            var request = MakeRequest("skills");
            var response = Client.Get<IEnumerable<SkillViewModel>>(request);

            response.Data.Count().ShouldBe(9);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void SkillController_GetById_Returns_CorrectSeedData()
        {
            var request = MakeRequest("skills/1");
            var response = Client.Get<SkillViewModel>(request);

            response.Data.Id.ShouldBe(SeedData.SeedSkills[0].Id);
            response.Data.Name.ShouldBe(SeedData.SeedSkills[0].Name);
            response.Data.CvId.ShouldBe(SeedData.SeedSkills[0].CvId);
            response.Data.MonthsExperience.ShouldBe(SeedData.SeedSkills[0].MonthsExperience);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public void SkillController_GetByIncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("skills/10");
            var response = Client.Get<SkillViewModel>(request);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }


        private SkillViewModel GetSkill()
        {
            var request = MakeRequest("skills/1");
            var response = Client.Get<SkillViewModel>(request);
            return response.Data;
        }

        [Fact]
        public void SkillController_Put_DifferentIds_Returns_BadRequest()
        {
            var request = MakeRequest("skills/1");
            SkillViewModel skillViewModelToTest = GetSkill();

            skillViewModelToTest.Id++;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Put<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void SkillController_Put_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("skills/10");
            SkillViewModel skillViewModelToTest = GetSkill();
            skillViewModelToTest.Id = 10;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Put<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void SkillController_Put_SameIds_Returns_Ok()
        {
            var request = MakeRequest("skills/1");
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
        public void SkillController_Post_NullSkill_Returns_BadRequest()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = null;

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void SkillController_Post_SkillHasId_Returns_InternalServerError()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = GetSkill();

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void SkillController_Post_Valid_Returns_Created()
        {
            int initialCount = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("skills")).Data.Count();

            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                Name = "Test Skill",
                CvId = 1,
                MonthsExperience = 20
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var results = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("skills"));
            results.Data.Count().ShouldBe(initialCount +1);
        }

        [Fact]
        public void SkillController_Post_InvalidName_Returns_BadRequest()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                Name = "Lorem ipsum dolor sit amet, consectetuer adipiscing",
                CvId = 1,
                MonthsExperience = 20
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void SkillController_Post_InvalidCvId_Returns_BadRequest()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                Name = "Test Skill",
                MonthsExperience = 20
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void SkillController_Post_InvalidMonthsExperience_Returns_BadRequest()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                Name = "Test Skill",
                CvId = 1,
                MonthsExperience = 601
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void SkillController_Post_NoMonthsExperience_Returns_BadRequest()
        {
            var request = MakeRequest("skills");
            SkillViewModel skillViewModelToTest = new SkillViewModel()
            {
                Name = "Test Skill",
                CvId = 1
            };

            request.RequestFormat = DataFormat.Json;
            request.AddBody(skillViewModelToTest);

            var response = Client.Post<SkillViewModel>(request);
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        [Fact]
        public void SkillController_Delete_IncorrectId_Returns_NotFound()
        {
            var request = MakeRequest("skills/10");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public void SkillController_Delete_CorrectId_Returns_Ok()
        {
            int initialCount = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("skills")).Data.Count();

            var request = MakeRequest("skills/1");

            var response = Client.Delete(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var results = Client.Get<IEnumerable<SkillViewModel>>(MakeRequest("skills"));
            results.Data.Count().ShouldBe(initialCount - 1);
        }

    }
}
