using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Project : DefaultModel
    {
        [MaxLength(1024)]
        [Required]
        public string Title { get; set; }
        [MaxLength(2048)]
        public string Link { get; set; }
        [MaxLength(2048)]
        public string CoverImage { get; set; }
        [Required]
        public string Description { get; set; }
        [MaxLength(2048)]
        public string RepositoryLink { get; set; }
    }
}
