using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.Models;

namespace PacTaskAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}