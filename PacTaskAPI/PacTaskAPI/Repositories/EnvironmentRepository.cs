using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PacTaskAPI.Data;
using PacTaskAPI.DTOs.Environment;
using PacTaskAPI.Interfaces;
using PacTaskAPI.Models;

namespace PacTaskAPI.Repositories
{
    public class EnvironmentRepository : IEnvironmentRepository
    {
        private readonly ApplicationDBContext _context;
        public EnvironmentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfUserHasEnvironment(int id, UserEntity user)
        {
            var envs = await GetUserEnvironments(user);
            return envs.Any(e => e.Id == id);
        }

        public async Task<EnvironmentEntity> Create(EnvironmentEntity environmentModel)
        {
            await _context.Environments.AddAsync(environmentModel);
            await _context.SaveChangesAsync();
            return environmentModel;
        }

        public async Task<EnvironmentEntity?> Delete(int id)
        {
            var environmentModel = await _context.Environments.FindAsync(id);
            if (environmentModel == null) return null;

            _context.Environments.Remove(environmentModel);
            await _context.SaveChangesAsync();

            return environmentModel;
        }

        public async Task<EnvironmentEntity?> GetEnvironmentById(int id)
        {
            return await _context.Environments.FindAsync(id);
        }

        public async Task<List<EnvironmentEntity>> GetUserEnvironments(UserEntity user)
        {
            return await _context.Environments.Where(e => e.UserId == user.Id).ToListAsync();
        }

        public async Task<EnvironmentEntity?> Update(int id, UpdateEnvironmentRequestDto environmentDto)
        {
            var environmentModel = await _context.Environments.FindAsync(id);
            if (environmentModel == null) return null;

            environmentModel.Title = environmentDto.Title;
            await _context.SaveChangesAsync();
            return environmentModel;
        }
    }
}