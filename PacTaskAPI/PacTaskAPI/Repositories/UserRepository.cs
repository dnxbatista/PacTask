using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PacTaskAPI.Data;
using PacTaskAPI.DTOs.User;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Models;

namespace PacTaskAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IPasswordService _passwordService;
        public UserRepository(ApplicationDBContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<User?> Delete(int id)
        {
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null) return null;
            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();
            return userModel;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> Login(LoginUserRequestDto userDto)
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username || u.Email == userDto.Email);
            if (loginUser == null) return null;
            if(!_passwordService.VerifyPassword(loginUser, loginUser.PasswordHash, userDto.Password)) return null;
            return loginUser;
        }

        public async Task<User> Register(User user, string rawPassword)
        {
            user.PasswordHash = _passwordService.HashPassword(user, rawPassword);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}