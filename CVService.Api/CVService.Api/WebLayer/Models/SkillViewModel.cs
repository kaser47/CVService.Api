using System.ComponentModel.DataAnnotations;

namespace CVService.Api.WebLayer.Models
{
    public class SkillViewModel : BaseViewModel
    {
        /// <summary>
        /// The id of the cv that the skill belongs too.
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
        /// The name of the skill e.g. C#
        /// </summary>
        [Required]
        [StringLength(30)]
        public override string Name { get; set; }

        /// <summary>
        /// Months experience with said skill
        /// </summary>
        /// <remarks>
        /// 50 years seemed a reasonable max.
        /// </remarks>
        [Required]
        [Range(1, 600)]
        public int MonthsExperience { get; set; }
    }
}