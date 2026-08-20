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
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public PasswordService(PasswordHasher<User> hasher)
        {
            _hasher = hasher;
        }

        public string HashPassword(User user, string rawPassword)
        {
            return _hasher.HashPassword(user, rawPassword);
        }

        public bool VerifyPassword(User user, string passwordHash, string rawPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, passwordHash, rawPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}