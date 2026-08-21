using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PacTaskAPI.DTOs.User
{
    public class UpdateUserRequestDto
    {
        [MaxLength(32, ErrorMessage = "Username cannot be longer than 32 characters")]
        public string? Username { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}