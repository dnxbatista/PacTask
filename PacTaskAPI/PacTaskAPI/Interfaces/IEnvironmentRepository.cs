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
        Task<List<EnvironmentEntity>> GetAll();
        Task<EnvironmentEntity?> GetById(int id);
        Task<EnvironmentEntity> Create(EnvironmentEntity environmentModel);
        Task<EnvironmentEntity?> Update(int id, UpdateEnvironmentRequestDto environmentDto);
        Task<EnvironmentEntity?> Delete(int id);
    }
}