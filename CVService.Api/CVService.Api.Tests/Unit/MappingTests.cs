using System.Linq;
using CVService.Api.CommonLayer;
using CVService.Api.DataLayer.Models;
using CVService.Api.WebLayer;
using CVService.Api.WebLayer.Models;
using Shouldly;
using Xunit;

namespace CVService.Api.Tests.Unit
{
    //TODO: Tech Test - I wanted to show the concept of mapping but ideally if AutoMapper was implemented testing wouldn't be necessary
    //as that's a well established package.
    public class MappingTests
    {
        [Fact]
        public void DataCv_MapsCorrectlyToWebCV_BackToDataCv()
        {
            Cv cv = SeedData.SeedCvs[0];
            cv.Skills = SeedData.SeedSkills.Where(s => s.CvId == 1).ToList();
            cv.CompanyHistories = SeedData.SeedCompanyHistories.Where(ch => ch.CvId == 1).ToList();

            CvViewModel cvViewModel = cv.MapToWebViewModel();

            Cv cvMapped = cvViewModel.MapToDataViewModel();

            cvMapped.Name.ShouldBe(cv.Name);
            cvMapped.Id.ShouldBe(cv.Id);
            cvMapped.EmailAddress.ShouldBe(cv.EmailAddress);
            for (int i = 0; i < cvMapped.Skills.Count; i++)
            {
                Skill skillMapped = cvMapped.Skills.ElementAt(i);
                Skill skill = cv.Skills.ElementAt(i);

                skillMapped.Id.ShouldBe(skill.Id);
                skillMapped.Name.ShouldBe(skill.Name);
                skillMapped.CvId.ShouldBe(skill.CvId);
                skillMapped.MonthsExperience.ShouldBe(skill.MonthsExperience);
            }

            for (int i = 0; i < cvMapped.Skills.Count; i++)
            {
                CompanyHistory companyHistoryMapped = cvMapped.CompanyHistories.ElementAt(i);
                CompanyHistory companyHistory = cv.CompanyHistories.ElementAt(i);

                companyHistoryMapped.Id.ShouldBe(companyHistory.Id);
                companyHistoryMapped.Name.ShouldBe(companyHistory.Name);
                companyHistoryMapped.CvId.ShouldBe(companyHistory.CvId);
                companyHistoryMapped.Description.ShouldBe(companyHistory.Description);
                companyHistoryMapped.StartDate.ShouldBe(companyHistory.StartDate);
                companyHistoryMapped.EndDate.ShouldBe(companyHistory.EndDate);
            }
        }

        [Fact]
        public void DataSkill_MapsCorrectlyToWebSkill_BackToDataSkill()
        {
            Skill skill = SeedData.SeedSkills[0];
            
            SkillViewModel skillViewModel = skill.MapToWebViewModel();

            Skill skillMapped = skillViewModel.MapToDataViewModel();

            skillMapped.Id.ShouldBe(skill.Id);
            skillMapped.Name.ShouldBe(skill.Name);
            skillMapped.CvId.ShouldBe(skill.CvId);
            skillMapped.MonthsExperience.ShouldBe(skill.MonthsExperience);
        }

        [Fact]
        public void DataCompanyHistory_MapsCorrectlyToWebCompanyHistory_BackToDataCompanyHistory()
        {
            CompanyHistory companyHistory = SeedData.SeedCompanyHistories[0];
     
            CompanyHistoryViewModel companyHistoryViewModel = companyHistory.MapToWebViewModel();

            CompanyHistory companyHistoryMapped = companyHistoryViewModel.MapToDataViewModel();

            companyHistoryMapped.Id.ShouldBe(companyHistory.Id);
            companyHistoryMapped.Name.ShouldBe(companyHistory.Name);
            companyHistoryMapped.CvId.ShouldBe(companyHistory.CvId);
            companyHistoryMapped.Description.ShouldBe(companyHistory.Description);
            companyHistoryMapped.StartDate.ShouldBe(companyHistory.StartDate);
            companyHistoryMapped.EndDate.ShouldBe(companyHistory.EndDate);
        }
    }
}
