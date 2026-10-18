using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Publication : DefaultModel
    {
        [Required]
        [MaxLength(1024)]
        public string Title { get; set; }
        [Required]
        [MaxLength(512)]
        public string Slug { get; set; }
        public string Summary { get; set; }
        [MaxLength(64)]
        public string Source { get; set; }
        [MaxLength(2048)]
        public string ExternalUrl { get; set; }
        [MaxLength(2048)]
        public string CoverImageUrl { get; set; }
        public DateTimeOffset? PublishedDate { get; set; }
    }
}
