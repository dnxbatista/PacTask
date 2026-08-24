using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PacTaskAPI.DTOs.User;
using PacTaskAPI.Extensions;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Mappers;

namespace PacTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        public UserController(IUserRepository userRepo, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = userDto.FromRegisterUserToUser();
            var newUser = await _userRepo.Register(userModel, userDto.Password);

            var loggedUserDto = new LoggedUserDto
            {
                Email = userModel.Email,
                Username = userModel.Username,
                Token = _tokenService.CreateToken(userModel)
            };

            return Ok(loggedUserDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userModel = await _userRepo.Login(userDto);
            if (userModel == null) return Unauthorized("Invalid credentials");

            var loggedUserDto = new LoggedUserDto
            {
                Email = userModel.Email,
                Username = userModel.Username,
                Token = _tokenService.CreateToken(userModel)
            };

            return Ok(loggedUserDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto userDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var username = User.GetUsername();
            var userModel = await _userRepo.GetUserByUsername(username);
            if (userModel == null) return BadRequest("User not found");

            var updatedUser = await _userRepo.Update(userModel, userDto);
            if (updatedUser == null) return BadRequest("Failed to update user");

            return Ok(updatedUser.FromUserToUserDto());
        }
    }
}