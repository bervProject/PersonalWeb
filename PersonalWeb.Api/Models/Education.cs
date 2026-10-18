using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Education : DefaultModel
    {
        [Required]
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        [Required]
        [MaxLength(512)]
        public string Degree { get; set; }
        [Required]
        [MaxLength(512)]
        public string Institution { get; set; }
        [MaxLength(256)]
        public string City { get; set; }
        [MaxLength(256)]
        public string Country { get; set; }
        [MaxLength(4096)]
        public string Description { get; set; }
    }
}
