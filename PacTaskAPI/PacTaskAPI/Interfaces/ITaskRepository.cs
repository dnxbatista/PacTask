using PacTaskAPI.DTOs.Task;
using PacTaskAPI.Models;

namespace PacTaskAPI.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskEntity>> GetAllTasksFromEnvironment(int environmentId);
        Task<TaskEntity?> GetTaskById(int taskId);
        Task<TaskEntity> CreateTaskInEnvironment(TaskEntity task);
        Task<TaskEntity?> UpdateTaskInEnvironment(TaskEntity task, UpdateTaskEntityRequestDto taskDto);
        Task<TaskEntity?> DeleteTaskInEnvironment(TaskEntity task);
        Task<bool> IsUserOwnerOfTask(TaskEntity task, UserEntity user);
    }
}
