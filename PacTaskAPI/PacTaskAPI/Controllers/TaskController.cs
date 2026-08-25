using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PacTaskAPI.DTOs.Task;
using PacTaskAPI.Extensions;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Mappers;
using PacTaskAPI.Models;

namespace PacTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IEnvironmentRepository _environmentRepo;
        private readonly IUserRepository _userRepo;
        public TaskController(ITaskRepository taskRepo, IEnvironmentRepository environmentRepo, IUserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _environmentRepo = environmentRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllTasks([FromRoute] int id) 
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            if (!await _environmentRepo.CheckIfUserHasEnvironment(id, user)) return Unauthorized("User does not have access to this environment");

            var tasks = await _taskRepo.GetAllTasksFromEnvironment(id);
            return Ok(tasks.Select(t => t.ToTaskDto()).ToList());
        }

        [HttpGet]
        [Route("{id:int}/unique")]
        [Authorize]
        public async Task<IActionResult> GetTaskById([FromRoute] int id)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            var task = await _taskRepo.GetTaskById(id);
            if (task == null) return NotFound("Task not found");

            if (!await _taskRepo.IsUserOwnerOfTask(task, user)) return Unauthorized("User is not the owner of this task");

            return Ok(task.ToTaskDto());
        }

        [HttpPost]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromRoute]int id,[FromBody] CreateTaskEntityRequestDto taskDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            if (!await _environmentRepo.CheckIfUserHasEnvironment(id, user)) return Unauthorized("User does not have access to this environment");

            var taskModel = taskDto.FromCreateRequestToTaskEntity(id);
            await _taskRepo.CreateTaskInEnvironment(taskModel);
            return CreatedAtAction(nameof(CreateTask), new { id = taskModel.Id }, taskDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody]UpdateTaskEntityRequestDto taskDto) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            var task = await _taskRepo.GetTaskById(id);
            if (task == null) return NotFound("Task not found");

            if (!await _taskRepo.IsUserOwnerOfTask(task, user)) return Unauthorized("User does not have access to this task");

            var taskUpdate = await _taskRepo.UpdateTaskInEnvironment(task, taskDto);
            if (taskUpdate == null) return BadRequest("Task unable to update");

            return Ok(taskUpdate.ToTaskDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromRoute] int id)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            var task = await _taskRepo.GetTaskById(id);
            if (task == null) return NotFound("Task not found");

            if (!await _taskRepo.IsUserOwnerOfTask(task, user)) return Unauthorized("User does not have access to this task");

            var taskDelete = await _taskRepo.DeleteTaskInEnvironment(task);
            if (taskDelete == null) return BadRequest("Error deleting task");

            return Ok(taskDelete.ToTaskDto());
        }
    }
}
