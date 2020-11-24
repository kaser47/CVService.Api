using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CVService.Api.CommonLayer.Abstracts;

namespace CVService.Api.DataLayer.Models
{
    public class CompanyHistory : IHasId, IHasCvId
    {
        public int Id { get; set; }

        [Required]
        public int CvId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}