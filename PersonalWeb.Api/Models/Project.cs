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
        [MaxLength(512)]
        public string Slug { get; set; }
        [MaxLength(4096)]
        public string TechStack { get; set; }
        [MaxLength(2048)]
        public string LiveUrl { get; set; }
        public bool IsPublished { get; set; } = true;
        public int OrderIndex { get; set; }
    }
}
