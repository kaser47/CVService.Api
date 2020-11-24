using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVService.Api.DataLayer;
using CVService.Api.DataLayer.Models;

namespace CVService.Api.CommonLayer
{
    //TODO: Tech Test - This class would most likely be replaced by the test project which would provide the seed data via the maintenance web requests. 
    public static class SeedData
    {
        public static List<Cv> SeedCvs = new List<Cv>()
        {
            new Cv
            {
                Id = 1,
                Name = "Ash Rhodes",
                EmailAddress = "kaser47@hotmail.com"
            },
            new Cv
            {
                Id = 2,
                Name = "John Smith",
                EmailAddress = "john.smith@gmail.com"
            },
            new Cv
            {
                Id = 3,
                Name = "Foo Bar",
                EmailAddress = "foo.bar@outlook.com"
            }
        };

        public static List<Skill> SeedSkills = new List<Skill>()
            {
                new Skill()
                {
                    Id = 1,
                    CvId = SeedCvs[0].Id,
                    Name = "C#",
                    MonthsExperience = 96
                },
                new Skill()
                {
                    Id = 2,
                    CvId = SeedCvs[0].Id,
                    Name = "JavaScript",
                    MonthsExperience = 24
                },
                new Skill()
                {
                    Id = 3,
                    CvId = SeedCvs[0].Id,
                    Name = "HTML",
                    MonthsExperience = 27
                },
                new Skill()
                {
                    Id = 4,
                    CvId = SeedCvs[1].Id,
                    Name = "Leadership",
                    MonthsExperience = 2
                },
                new Skill()
                {
                    Id = 5,
                    CvId = SeedCvs[1].Id,
                    Name = "Problem Solving",
                    MonthsExperience = 15
                },
                new Skill()
                {
                    Id = 6,
                    CvId = SeedCvs[1].Id,
                    Name = "Communication",
                    MonthsExperience = 64
                },
                new Skill()
                {
                    Id = 7,
                    CvId = SeedCvs[2].Id,
                    Name = "Creativity",
                    MonthsExperience = 196
                },
                new Skill()
                {
                    Id = 8,
                    CvId = SeedCvs[2].Id,
                    Name = "Cooking",
                    MonthsExperience = 45
                },
                new Skill()
                {
                    Id = 9,
                    CvId = SeedCvs[2].Id,
                    Name = "Fishing",
                    MonthsExperience = 39
                }
            };

       public static List<CompanyHistory> SeedCompanyHistories = new List<CompanyHistory>()
            {
                new CompanyHistory()
                {
                    Id = 1,
                    CvId = SeedCvs[0].Id,
                    Name = "EMIS",
                    Description = "My role at EMIS involved me fixing bugs",
                    StartDate = DateTime.ParseExact("01/01/2001", "dd/MM/yyyy", null),
                    EndDate = DateTime.ParseExact("01/01/2002", "dd/MM/yyyy", null)
                },
                new CompanyHistory()
                {
                    Id = 2,
                    CvId = SeedCvs[0].Id,
                    Name = "Virtual College",
                    Description = "My role at Virtual College involved me fixing bugs",
                    StartDate = DateTime.ParseExact("01/01/2002", "dd/MM/yyyy", null),
                    EndDate = DateTime.ParseExact("01/01/2003", "dd/MM/yyyy", null)
                },
                new CompanyHistory()
                {
                    Id = 3,
                    CvId = SeedCvs[0].Id,
                    Name = "Google",
                    Description = "My role at Google involved me fixing bugs",
                    StartDate = DateTime.ParseExact("01/01/2003", "dd/MM/yyyy", null),
                    EndDate = null
                },
                new CompanyHistory()
                {
                    Id = 4,
                    CvId = SeedCvs[1].Id,
                    Name = "Team Leader - Facebook",
                    Description = "My role at facebook involved me leading a team of developers",
                    StartDate = DateTime.ParseExact("01/01/2003", "dd/MM/yyyy", null),
                    EndDate = DateTime.ParseExact("01/01/2012", "dd/MM/yyyy", null),
                },
                new CompanyHistory()
                {
                    Id = 5,
                    CvId = SeedCvs[1].Id,
                    Name = "CTO - Google",
                    Description = "My current role at Google involved me managing all the developers",
                    StartDate = DateTime.ParseExact("01/01/2012", "dd/MM/yyyy", null),
                    EndDate = null
                },
                new CompanyHistory()
                {
                    Id = 6,
                    CvId = SeedCvs[2].Id,
                    Name = "McDonalds",
                    Description = "I work as a chef at McDonalds",
                    StartDate = DateTime.ParseExact("01/01/2016", "dd/MM/yyyy", null),
                    EndDate = null
                },
            };

       public static void AddTestData(ApiContext context)
       {
           context.Cvs.AddRange(SeedData.SeedCvs);
           context.Skills.AddRange(SeedData.SeedSkills);
           context.CompanyHistories.AddRange(SeedData.SeedCompanyHistories);

           context.SaveChanges();
       }
    }
}
