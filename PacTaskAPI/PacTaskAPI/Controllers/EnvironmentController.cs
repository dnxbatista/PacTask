using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PacTaskAPI.DTOs.Environment;
using PacTaskAPI.Extensions;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Mappers;
using PacTaskAPI.Models;
using PacTaskAPI.Repositories;

namespace PacTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvironmentController : ControllerBase
    {
        private readonly IEnvironmentRepository _environmentRepo;
        private readonly IUserRepository _userRepo;
        
        public EnvironmentController(IEnvironmentRepository environmentRepo, IUserRepository userRepo)
        {
            _environmentRepo = environmentRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUserEnvironments()
        {
            var username = User.GetUsername();
            var userModel = await _userRepo.GetUserByUsername(username);
            if (userModel == null) return NotFound("User not found");

            var envs = await _environmentRepo.GetUserEnvironments(userModel);
            return Ok(envs.Select(s => s.ToEnvironmentDto()).ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEnvironment([FromBody] CreateEnvironmentRequestDto environmentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            var environmentModel = environmentDto.FromCreateToEnvironmentDto(user.Id);
            await _environmentRepo.Create(environmentModel);
            return CreatedAtAction(nameof(CreateEnvironment), new { id = environmentModel.Id }, environmentDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateEnvironment([FromRoute]int id,[FromBody] UpdateEnvironmentRequestDto environmentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return NotFound("User not found");

            // Check if the logged user is owner of the environment that he wants to update
            if (!await _environmentRepo.CheckIfUserHasEnvironment(id, user)) return Unauthorized("You cant update this env");

            var updatedEnvironment = await _environmentRepo.Update(id, environmentDto);
            if (updatedEnvironment == null) return NotFound("Environment not found");

            return Ok(updatedEnvironment.ToEnvironmentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteEnvironment([FromRoute]int id)
        {
            var user = await _userRepo.GetUserByUsername(User.GetUsername());
            if (user == null) return BadRequest("User not found");

            if (!await _environmentRepo.CheckIfUserHasEnvironment(id, user)) return Unauthorized("You cant update this env");

            var userToDelete = await _environmentRepo.Delete(id);
            if (userToDelete == null) return NotFound("Environment not found");
            return Ok(userToDelete.ToEnvironmentDto());
        }
    }
}