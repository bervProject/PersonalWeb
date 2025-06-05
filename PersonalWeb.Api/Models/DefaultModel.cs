using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{
    public abstract record DefaultModel : IAuditable, IDraftable
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(216)]
        public string CreateBy { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        [MaxLength(216)]
        public string UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(512)]
        public string DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
        public bool IsDraft { get; set; }
    }
}
