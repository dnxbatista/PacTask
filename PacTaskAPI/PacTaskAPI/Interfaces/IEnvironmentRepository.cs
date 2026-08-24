using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacTaskAPI.DTOs.Environment;
using PacTaskAPI.Models;

namespace PacTaskAPI.Interfaces
{
    public interface IEnvironmentRepository
    {
        Task<EnvironmentEntity?> GetById(int id);
        Task<List<EnvironmentEntity>> GetUserEnvironments(UserEntity user);
        Task<bool> CheckIfUserHasEnvironment(int id, UserEntity user);
        Task<EnvironmentEntity> Create(EnvironmentEntity environmentModel);
        Task<EnvironmentEntity?> Update(int id, UpdateEnvironmentRequestDto environmentDto);
        Task<EnvironmentEntity?> Delete(int id);
    }
}