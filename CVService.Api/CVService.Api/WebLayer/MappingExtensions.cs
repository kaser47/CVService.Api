using System.Linq;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.WebLayer
{
    public static class MappingExtensions
    {
        public static Skill MapToDataViewModel(this Models.SkillViewModel skillViewModel)
        {
            Skill skill = new Skill()
            {
                Id = skillViewModel.Id,
                Name = skillViewModel.Name,
                CvId = skillViewModel.CvId.Value,
                MonthsExperience = skillViewModel.MonthsExperience
            };

            return skill;
        }

        public static CompanyHistory MapToDataViewModel(this Models.CompanyHistoryViewModel companyHistoryViewModel)
        {
            CompanyHistory companyHistory = new CompanyHistory()
            {
                Id = companyHistoryViewModel.Id,
                Name = companyHistoryViewModel.Name,
                CvId = companyHistoryViewModel.CvId.Value,
                Description = companyHistoryViewModel.Description,
                StartDate = companyHistoryViewModel.StartDate.Value,
                EndDate = companyHistoryViewModel.EndDate
            };

            return companyHistory;
        }

        public static Cv MapToDataViewModel(this Models.CvViewModel cvViewModel)
        {
            Cv cv = new Cv()
            {
                Id = cvViewModel.Id,
                Name = cvViewModel.Name,
                EmailAddress = cvViewModel.EmailAddress,
                Skills = cvViewModel.Skills?.Select(s => s.MapToDataViewModel()).ToList(),
                CompanyHistories = cvViewModel.CompanyHistories?.Select(ch => ch.MapToDataViewModel()).ToList()
            };

            return cv;
        }

        public static Models.SkillViewModel MapToWebViewModel(this Skill skill)
        {
            Models.SkillViewModel skillViewModel = new Models.SkillViewModel()
            {
                Id = skill.Id,
                Name = skill.Name,
                CvId = skill.CvId,
                MonthsExperience = skill.MonthsExperience
            };

            return skillViewModel;
        }

        public static Models.CompanyHistoryViewModel MapToWebViewModel(this CompanyHistory companyHistory)
        {
            Models.CompanyHistoryViewModel companyHistoryViewModel = new Models.CompanyHistoryViewModel()
            {
                Id = companyHistory.Id,
                Name = companyHistory.Name,
                CvId = companyHistory.CvId,
                Description = companyHistory.Description,
                StartDate = companyHistory.StartDate,
                EndDate = companyHistory.EndDate
            };

            return companyHistoryViewModel;
        }

        public static Models.CvViewModel MapToWebViewModel(this Cv cv)
        {
            Models.CvViewModel cvViewModel = new Models.CvViewModel()
            {
                Id = cv.Id,
                Name = cv.Name,
                EmailAddress = cv.EmailAddress,
                Skills = cv.Skills?.Select(s => s.MapToWebViewModel()).ToList(),
                CompanyHistories = cv.CompanyHistories?.Select(ch => ch.MapToWebViewModel()).ToList()
            };

            return cvViewModel;
        }
    }
}
