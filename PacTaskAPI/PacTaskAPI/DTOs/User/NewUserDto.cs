using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.Models;

namespace PacTaskAPI.DTOs.User
{
    public class NewUserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public ICollection<EnvironmentEntity> Environments { get; set; } = new List<EnvironmentEntity>();
        [Required]
        public string Token {get; set; } = string.Empty;
    }
}