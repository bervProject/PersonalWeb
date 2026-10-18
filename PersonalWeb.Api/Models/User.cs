using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWeb.Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required]
        [MaxLength(512)]
        public string PasswordHash { get; set; }
        [MaxLength(64)]
        public string Role { get; set; } = "admin";
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
