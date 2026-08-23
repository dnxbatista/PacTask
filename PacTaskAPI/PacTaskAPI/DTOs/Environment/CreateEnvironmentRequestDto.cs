using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PacTaskAPI.DTOs.Environment
{
    public class CreateEnvironmentRequestDto
    {
        [Required]
        [MaxLength(32, ErrorMessage = "Title max length is 32 characters")]
        public string Title { get; set; } = string.Empty;
    }
}