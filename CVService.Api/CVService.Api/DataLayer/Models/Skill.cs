using System.ComponentModel.DataAnnotations;
using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.DataLayer.Models
{
    public class Skill : IHasId, IHasCvId
    {
        public int Id { get; set; }

        [Required]
        public int CvId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Range(1, 600)]
        public int MonthsExperience { get; set; }
    }
}