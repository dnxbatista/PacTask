using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.DTOs.User;
using PacTaskAPI.Models;

namespace PacTaskAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserByUsername(string username);
        Task<UserEntity?> GetUserByEmail(string email);
        Task<UserEntity?> Login(LoginUserRequestDto userDto);
        Task<UserEntity> Register(UserEntity user, string rawPassword);
        Task<UserEntity?> Update(UserEntity user, UpdateUserRequestDto userDto);
    }
}