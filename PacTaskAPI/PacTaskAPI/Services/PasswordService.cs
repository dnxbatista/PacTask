using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Models;

namespace PacTaskAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<UserEntity> _hasher = new PasswordHasher<UserEntity>();

        public PasswordService(PasswordHasher<UserEntity> hasher)
        {
            _hasher = hasher;
        }

        public string HashPassword(UserEntity user, string rawPassword)
        {
            return _hasher.HashPassword(user, rawPassword);
        }

        public bool VerifyPassword(UserEntity user, string passwordHash, string rawPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, passwordHash, rawPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}