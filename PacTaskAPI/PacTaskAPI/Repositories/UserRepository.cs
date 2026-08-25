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

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<UserEntity?> Login(LoginUserRequestDto userDto)
        {
            var loginUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username || u.Email == userDto.Email);
            if (loginUser == null) return null;
            if(!_passwordService.VerifyPassword(loginUser, loginUser.PasswordHash, userDto.Password)) return null;
            return loginUser;
        }

        public async Task<UserEntity> Register(UserEntity user, string rawPassword)
        {
            user.PasswordHash = _passwordService.HashPassword(user, rawPassword);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<UserEntity?> Update(UserEntity user, UpdateUserRequestDto userDto)
        {
            var userModel = user;
            
            if (userModel == null) return null;

            if (userDto.Username != null)
            {
                userModel.Username = userDto.Username;
            }
            if (userDto.Email != null)
            {
                userModel.Email = userDto.Email;
            }
            if (userDto.Password != null)
            {
                userModel.PasswordHash = _passwordService.HashPassword(userModel, userDto.Password);
            }
            await _context.SaveChangesAsync();
            return userModel;
        }
    }
}