using PacTaskAPI.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PacTaskAPI.Models
{
    [Table("Tasks")]
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
        public int EnvironmentId { get; set; }
        public EnvironmentEntity? Environment { get; set; }
    }
}