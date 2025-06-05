using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Blog : DefaultModel
    {
        [Required]
        [MaxLength(1024)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [MaxLength(128)]
        [Required]
        public string Icon { get; set; }
        [MaxLength(2048)]
        [Required]
        public string Link { get; set; }
    }
}