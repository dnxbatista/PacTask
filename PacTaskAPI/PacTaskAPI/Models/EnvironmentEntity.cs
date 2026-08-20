using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PacTaskAPI.Models
{
    [Table("Environments")]
    public class EnvironmentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}