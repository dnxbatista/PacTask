using Microsoft.EntityFrameworkCore;
using PacTaskAPI.Data;
using PacTaskAPI.DTOs.Task;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Models;

namespace PacTaskAPI.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IEnvironmentRepository _environmentRepo;
        public TaskRepository(IEnvironmentRepository environmentRepo, ApplicationDBContext context)
        {
            _environmentRepo = environmentRepo;
            _context = context;
        }

        public async Task<TaskEntity> CreateTaskInEnvironment(TaskEntity task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity?> DeleteTaskInEnvironment(TaskEntity task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public Task<List<TaskEntity>> GetAllTasksFromEnvironment(int environmentId)
        {
            var tasks = _context.Tasks.Where(t => t.EnvironmentId == environmentId).ToListAsync();
            return tasks;
        }

        public async Task<TaskEntity?> GetTaskById(int taskId)
        {
            return await _context.Tasks.FindAsync(taskId);
        }

        public async Task<bool> IsUserOwnerOfTask(TaskEntity task, UserEntity user)
        {
            var taskEnvironment = await _context.Environments.FindAsync(task.EnvironmentId);
            var isOwner = await _environmentRepo.CheckIfUserHasEnvironment(task.EnvironmentId, user);
            return isOwner;
        }

        public async Task<TaskEntity?> UpdateTaskInEnvironment(TaskEntity task, UpdateTaskEntityRequestDto taskDto)
        {
            var taskModel = await _context.Tasks.FindAsync(task.Id);
            if (taskModel == null) return null;

            taskModel.Title = taskDto.Title;
            taskModel.Description = taskDto.Description;
            taskModel.Status = taskDto.Status;

            await _context.SaveChangesAsync();
            return taskModel;
        }
    }
}
