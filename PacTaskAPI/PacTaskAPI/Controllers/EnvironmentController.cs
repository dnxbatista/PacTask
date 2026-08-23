using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PacTaskAPI.Interfaces;

namespace PacTaskAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnvironmentController : ControllerBase
    {
        private readonly IEnvironmentRepository _environmentRepo;
        public EnvironmentController(IEnvironmentRepository environmentRepo)
        {
            _environmentRepo = environmentRepo;
        }
    }
}