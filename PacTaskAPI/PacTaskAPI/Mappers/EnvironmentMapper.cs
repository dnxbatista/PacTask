using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.DTOs.Environment;
using PacTaskAPI.Models;

namespace PacTaskAPI.Mappers
{
    public static class EnvironmentMapper
    {
        public static EnvironmentDto ToEnvironmentDto(this EnvironmentEntity environmentModel)
        {
            return new EnvironmentDto
            {
                Id = environmentModel.Id,
                Title = environmentModel.Title
            };
        }

        public static EnvironmentEntity FromCreateToEnvironmentDto(this CreateEnvironmentRequestDto environmentDto, int userId)
        {
            return new EnvironmentEntity
            {
                Title = environmentDto.Title,
                UserId = userId
            };
        }
    }
}