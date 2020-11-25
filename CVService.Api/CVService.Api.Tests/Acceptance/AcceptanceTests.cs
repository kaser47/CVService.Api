using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CVService.Api.CommonLayer.Abstracts;
using CVService.Api.Tests.Integration;
using CVService.Api.WebLayer.Abstracts;
using CVService.Api.WebLayer.Models;
using RestSharp;
using Shouldly;
using Xunit;

namespace CVService.Api.Tests.Acceptance
{
    [Collection("SerialTests")]
    public class AcceptanceTests : IDisposable
    {
        public AcceptanceTests()
        {
            WipeData();
        }

        ////TODO: Tech Test - In a real-world application this value would be taken from an appsettings config
        ////file as we wouldn't hardcode any baseurls in the app (this wouldn't work if we ran the tests on a CI server).
        ////This would be the same for say the private access token, we'd store that in the app config file.
        private RestClient client = new RestClient("https://localhost:44341/api/v1/");

        ////This is a complete run through of the entire application from a clean state
        ////Unfortunately due to time constraints all endpoints are not being tested by this class, about 80% are.
        [Fact]
        public void Demo()
        {
            //1. Do gets accross all endpoints and assert all endpoints return an empty list
            AssertCountEqual(MakeGetAllRequest<CvViewModel>("cv"), 0);
            AssertCountEqual(MakeGetAllRequest<SkillViewModel>("Skills"), 0);
            AssertCountEqual(MakeGetAllRequest<CompanyHistoryViewModel>("company-histories"), 0);

            //2. A series of CRUD operations
            List<CvViewModel> cvs = GenerateCvs(5, 3, 3);
            List<SkillViewModel> skills = GenerateSkills(1, 10);
            List<CompanyHistoryViewModel> companyHistories = GenerateCompanyHistories(2, 10);

            IList<CvViewModel> createdCvs = new List<CvViewModel>();
            IList<SkillViewModel> createdSkills = new List<SkillViewModel>();
            IList<CompanyHistoryViewModel> createdCompanyHistories = new List<CompanyHistoryViewModel>();

            //Post them
            cvs.ForEach(cv => createdCvs.Add(MakePostRequest("cv", cv).Data));
            skills.ForEach(skill => createdSkills.Add(MakePostRequest("skills", skill).Data));
            companyHistories.ForEach(companyHistory => createdCompanyHistories.Add(MakePostRequest("company-histories", companyHistory).Data));

            //Update a few
            var updatedCv = UpdateName(createdCvs.Single(x => x.Id == 1), "cv/1");
            var updatedSkill = UpdateName(createdSkills.Single(x => x.Id == 16), "skills/16");
            var updatedCompanyHistory = UpdateName(createdCompanyHistories.Single(x => x.Id == 16), "company-histories/16");

            //Check names
            updatedCv.Name.ShouldContain("Updated");
            updatedSkill.Name.ShouldContain("Updated");
            updatedCompanyHistory.Name.ShouldContain("Updated");

            //Delete some and check for cascading 
            MakeDeleteRequest($"cv/{updatedCv.Id}");
            MakeDeleteRequest($"skills/{updatedCv.Id}");
            MakeDeleteRequest($"company-histories/{updatedCv.Id}");

            AssertCountEqual(MakeGetAllRequest<CvViewModel>("cv"), 4);
            AssertCountEqual(MakeGetAllRequest<SkillViewModel>("skills"), 12);
            AssertCountEqual(MakeGetAllRequest<CompanyHistoryViewModel>("company-histories"), 22);
            //Success!
        }

        #region TestHelpers

        private void AssertCountEqual<T>(IRestResponse<IEnumerable<T>> data, int expected)
        {
            data.Data?.Count().ShouldBe(expected);
        }

        private T UpdateName<T>(T entity, string path) where T : INameable
        {
            entity.Name = entity.Name + "Updated";
            var result = MakePutRequest<T>(path, entity);
            return result.Data;
        }

        public RestRequest MakeRequest(string path)
        {
            RestRequest request = new RestRequest(path);
            request.AddHeader("PrivateAccessToken", "84157CEC-965E-4680-BDD8-AFFD81AD0D2A");
            return request;
        }

        private IRestResponse<IEnumerable<T>> MakeGetAllRequest<T>(string path)
        {
            return client.Get<IEnumerable<T>>(MakeRequest(path));
        }

        private IRestResponse<T> MakePostRequest<T>(string path, T entity)
        {
            var request = MakeRequest(path);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(entity);

            return client.Post<T>(request);
        }

        private IRestResponse<T> MakePutRequest<T>(string path, T entity)
        {
            var request = MakeRequest(path);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(entity);

            return client.Put<T>(request);
        }

        private IRestResponse MakeDeleteRequest(string path)
        {
            return client.Delete(MakeRequest(path));
        }


        private List<CvViewModel> GenerateCvs(int numberOfCvs, int numberOfSkills, int numberOfCompanyHistories)
        {
            List<CvViewModel> cvs = new List<CvViewModel>();
            string uniqueValue = Guid.NewGuid().ToString().Substring(0, 5);

            for (int i = 0; i < numberOfCvs; i++)
            {
                CvViewModel cvViewModel = new CvViewModel()
                {
                    Name = $"Test CV {uniqueValue} - {i}",
                    EmailAddress = $"email{i}@address.com",
                };

                Random rand = new Random();

                for (int j = 0; j < numberOfSkills; j++)
                {
                    int experience = rand.Next(1, 600);
                    cvViewModel.Skills.Add(new SkillViewModel()
                    {
                        Name = $"Test Skill {uniqueValue} - {j}",
                        MonthsExperience = experience,
                        CvId = 1
                    });
                }

                for (int j = 0; j < numberOfCompanyHistories; j++)
                {
                    cvViewModel.CompanyHistories.Add(new CompanyHistoryViewModel()
                    {
                        Name = $"CH{uniqueValue}-{j}",
                        Description = $"Test Description {j}",
                        StartDate = DateTime.Now.AddYears(-1).AddMonths(j),
                        EndDate = DateTime.Now.AddYears(-1).AddMonths(j + 1),
                        CvId = 1
                    });
                }

                cvs.Add(cvViewModel);
            }
            return cvs;
        }


        private List<SkillViewModel> GenerateSkills(int cvId, int numberOfSkills)
        {
            List<SkillViewModel> skills = new List<SkillViewModel>();
            string uniqueValue = Guid.NewGuid().ToString().Substring(0, 5);

            Random rand = new Random();
            for (int i = 0; i < numberOfSkills; i++)
            {
                int experience = rand.Next(1, 600);
                SkillViewModel skillViewModel = new SkillViewModel()
                {
                    Name = $"Test Skill {uniqueValue} - {i}",
                    CvId = cvId,
                    MonthsExperience = experience
                };

                skills.Add(skillViewModel);
            }

            return skills;
        }

        private List<CompanyHistoryViewModel> GenerateCompanyHistories(int cvId, int numberOfCompanyHistories)
        {
            List<CompanyHistoryViewModel> companyHistories = new List<CompanyHistoryViewModel>();
            string uniqueValue = Guid.NewGuid().ToString().Substring(0, 5);

            for (int i = 0; i < numberOfCompanyHistories; i++)
            {
                CompanyHistoryViewModel companyHistoryViewModel = new CompanyHistoryViewModel()
                {
                    Name = $"CH{uniqueValue}-{i}",
                    Description = $"Test Description {i}",
                    CvId = cvId,
                    StartDate = DateTime.Now.AddYears(-1).AddMonths(i),
                    EndDate = DateTime.Now.AddYears(-1).AddMonths(i + 1)
                };

                companyHistories.Add(companyHistoryViewModel);
            }

            return companyHistories;
        }
        #endregion
        public void Dispose()
        {
            WipeData();
        }

        private static void WipeData()
        {
            var client = new RestClient("https://localhost:44341/Maintenance");
            var request = new RestRequest("WipeData");
            client.Get(request);
        }
    }
}
