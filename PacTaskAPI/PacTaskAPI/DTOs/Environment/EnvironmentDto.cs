using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.Models;

namespace PacTaskAPI.DTOs.Environment
{
    public class EnvironmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
        public int UserId { get; set; }
    }
}