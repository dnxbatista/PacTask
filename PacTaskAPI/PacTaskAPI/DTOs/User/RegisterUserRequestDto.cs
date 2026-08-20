using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PacTaskAPI.DTOs.User
{
    public class RegisterUserRequestDto
    {
        [Required]
        [MaxLength(32, ErrorMessage = "Username cannot be longer than 32 characters")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}