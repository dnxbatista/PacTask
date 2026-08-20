using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.Models;

namespace PacTaskAPI.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(User user, string rawPassword);
        bool VerifyPassword(User user, string passwordHash, string rawPassword);
    }
}