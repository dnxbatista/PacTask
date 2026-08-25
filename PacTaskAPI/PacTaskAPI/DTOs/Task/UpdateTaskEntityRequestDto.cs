using PacTaskAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace PacTaskAPI.DTOs.Task
{
    public class UpdateTaskEntityRequestDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public TaskEntityStatus Status { get; set; } = TaskEntityStatus.NotDone;
    }
}
