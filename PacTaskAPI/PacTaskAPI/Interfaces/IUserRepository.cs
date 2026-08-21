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
        Task<List<User>> GetAll(); // JUST FOR DEBUG
        Task<User?> GetUser(int id);
        Task<User?> Login(LoginUserRequestDto userDto);
        Task<User> Register(User user, string rawPassword);
        Task<User?> Update(int id, UpdateUserRequestDto userDto);
        Task<User?> Delete(int id);
    }
}