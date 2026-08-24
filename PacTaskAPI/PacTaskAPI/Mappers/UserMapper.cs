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
        public static UserDto FromUserToUserDto(this UserEntity user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Environments = user.Environments
            };
        }

        public static UserEntity FromRegisterUserToUser(this RegisterUserRequestDto userDto)
        {
            return new UserEntity
            {
                Username = userDto.Username,
                Email = userDto.Email,
            };
        }
    }
}