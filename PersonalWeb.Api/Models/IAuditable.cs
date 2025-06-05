using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalWeb.Api.Models
{

    /**
    * Abstract class to define about audit columns
    */
    public interface IAuditable
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(512)]
        public string CreateBy { get; set; }
        [Required]
        public DateTimeOffset CreatedDate { get; set; }
        [MaxLength(512)]
        public string UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(512)]
        public string DeletedBy { get; set; }
        public DateTimeOffset? DeletedDate { get; set; }
    }

}