using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Profile : DefaultModel
    {
        [Required]
        [MaxLength(512)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Titles { get; set; }
        public string Bio { get; set; }
        [MaxLength(2048)]
        public string ResumeUrl { get; set; }
    }
}
