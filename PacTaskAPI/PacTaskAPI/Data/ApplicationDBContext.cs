using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PacTaskAPI.Models;

namespace PacTaskAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EnvironmentEntity> Environments { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EnvironmentEntity>()
            .HasOne(e => e.User)
            .WithMany(u => u.Environments)
            .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.Environment)
            .WithMany(e => e.Tasks)
            .HasForeignKey(t => t.EnvironmentId);
        }
    }
}