using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CVService.Api.WebLayer.Models
{
    public class CompanyHistoryViewModel : BaseViewModel, IValidatableObject
    {
        /// <summary>
        /// The id of the cv that the company history belongs too.
        /// </summary>
        /// <remarks>
        /// Used to link to a CV Entity
        /// </remarks>
        [Required]
        public int? CvId { get; set; }
        //TODO: Tech Test - This required nullable syntax only exists because im reusing the viewmodel
        //instead of splitting out the models per method endpoint and without the nullable it causes
        //issues when no Id is set and the value gets set to 0. https://stackoverflow.com/questions/6662976/required-attribute-for-an-integer-value

        /// <summary>
        /// The name of the company
        /// </summary>
        [Required]
        [StringLength(30)]
        public override string Name { get; set; }

        /// <summary>
        /// The brief summary of work carried about at the company
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        /// <remarks>Please note End date cannot be earlier in time then start date.</remarks>>
        public DateTime? EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (EndDate.HasValue && EndDate < StartDate)
            {
                validationResults.Add(new ValidationResult("End date cannot be earlier then start date"));
            }

            return validationResults;
        }
    }
}