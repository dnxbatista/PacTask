using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PacTaskAPI.DTOs.User;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Mappers;

namespace PacTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordService _passwordService;
        public UserController(IUserRepository userRepo, IPasswordService passwordService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allUsers = await _userRepo.GetAll();
            var allUsersDto = allUsers.Select(u => u.FromUserToUserDto());
            return Ok(allUsersDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userModel = await _userRepo.GetUser(id);
            if(userModel == null) return NotFound("User requested has no been found!");
            return Ok(userModel);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = userDto.FromRegisterUserToUser();
            var newUser = await _userRepo.Register(userModel, userDto.Password);

            return Ok(newUser.FromUserToUserDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = await _userRepo.Login(userDto);
            if (userModel == null) return BadRequest();
            return Ok(userModel.FromUserToUserDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userModel = await _userRepo.Delete(id);
            if (userModel == null) return NotFound("User has not no been found!");
            return Ok($"Delete user of id: {userModel.Id}");
        }
    }
}