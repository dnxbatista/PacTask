using PacTaskAPI.DTOs.Task;
using PacTaskAPI.Models;

namespace PacTaskAPI.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto ToTaskDto(this TaskEntity taskModel)
        {
            return new TaskDto
            {
                Id = taskModel.Id,
                Title = taskModel.Title,
                Description = taskModel.Description,
                Status = taskModel.Status,
                EnvironmentId = taskModel.EnvironmentId,
            };
        }

        public static TaskEntity FromCreateRequestToTaskEntity(this CreateTaskEntityRequestDto taskDto, int environmentId)
        {
            return new TaskEntity
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = 0,
                EnvironmentId = environmentId
            };
        }
    }
}
