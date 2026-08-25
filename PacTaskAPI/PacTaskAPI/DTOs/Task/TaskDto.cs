using PacTaskAPI.Enums;
using PacTaskAPI.Models;

namespace PacTaskAPI.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
        public int EnvironmentId { get; set; }
    }
}
