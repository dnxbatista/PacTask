using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.DTOs.User;
using PacTaskAPI.Models;

namespace PacTaskAPI.Mappers
{
    public static class UserMapper
    {
        public static UserDto FromUserToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Environments = user.Environments
            };
        }

        public static User FromRegisterUserToUser(this RegisterUserRequestDto userDto)
        {
            return new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
            };
        }
    }
}