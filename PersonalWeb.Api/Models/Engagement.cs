using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public record Engagement : DefaultModel
    {
        [Required]
        [MaxLength(1024)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(256)]
        public string EngagementType { get; set; }
        [MaxLength(1024)]
        public string TopicOrRole { get; set; }
        [Required]
        public DateTimeOffset EventDate { get; set; }
        [MaxLength(2048)]
        public string Location { get; set; }
        [MaxLength(2048)]
        public string ExternalUrl { get; set; }
    }
}
