using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Experience : DefaultModel
    {
        [Required]
        [MaxLength(1024)]
        public string CompanyName { get; set; }
        [Required]
        [MaxLength(1024)]
        public string RoleTitle { get; set; }
        [MaxLength(2048)]
        public string Location { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string DescriptionHtml { get; set; }
        [MaxLength(4096)]
        public string TechnologiesUsed { get; set; }
    }
}
