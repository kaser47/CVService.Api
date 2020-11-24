using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CVService.Api.WebLayer.Models
{

    public class CvViewModel : BaseViewModel, IValidatableObject
    {
        public CvViewModel()
        {
            Skills = new List<SkillViewModel>();
            CompanyHistories = new List<CompanyHistoryViewModel>();
        }

        /// <summary>
        /// The name of the person who the CV belongs.
        /// </summary>
        [Required]
        [StringLength(50)]
        public override string Name { get; set; }

        /// <summary>
        /// The email of the person who the CV belongs.
        /// </summary>
        ///<remarks>Has to be valid email format.</remarks>
        [EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The list of skills that belong to this CV.
        /// </summary>
        /// <remarks>You cannot have multiple skills of the same name</remarks>>
        public List<SkillViewModel> Skills { get; set; }

        /// <summary>
        /// The list of company history information that belong to this CV.
        /// </summary>
        /// <remarks>You cannot have multiple company histories with no end date</remarks>>
        public List<CompanyHistoryViewModel> CompanyHistories { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            var duplicateSkills = Skills?.GroupBy(x => x.Name)
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key)
                    .ToList();

            if (duplicateSkills != null && duplicateSkills.Any())
            {
                validationResults.Add(new ValidationResult("You cant have duplicate skills in a Cv"));
            }
                
            //TODO: Tech Test - I know technically its feasible for people to have more than one current job but this felt like a good example of additional validation
            var invalidCompanyHistories = CompanyHistories?.Count(x => x.EndDate == null) > 1;
            if (invalidCompanyHistories)
            {
                validationResults.Add(new ValidationResult("You cant have more than one current job in a CV"));
            }
            
            return validationResults;
        }

    }
}
