using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.DataLayer.Models
{
    public class Cv : IHasId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

        public ICollection<Skill> Skills { get; set; }

        public ICollection<CompanyHistory> CompanyHistories { get; set; }
    }
}
