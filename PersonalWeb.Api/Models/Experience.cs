using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Experience : DefaultModel
    {
        [MaxLength(1024)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [MaxLength(2048)]
        public string RepositoryLink { get; set; }
        [Required]
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Location { get; set; }
        [Required]
        [MaxLength(256)]
        public string ExperienceType { get; set; }
        [Required]
        [MaxLength(1024)]
        public string EntityName { get; set; }
        [Required]
        [MaxLength(2048)]
        public string EntityLink { get; set; }
    }
}
