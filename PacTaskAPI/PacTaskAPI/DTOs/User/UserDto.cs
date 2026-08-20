using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.Models;

namespace PacTaskAPI.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<EnvironmentEntity> Environments { get; set; } = new List<EnvironmentEntity>();
    }
}