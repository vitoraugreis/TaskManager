using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using System;
using System.IO;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbName = "tasks.db";

                string exePath = AppDomain.CurrentDomain.BaseDirectory;
                Directory.CreateDirectory(exePath);
                string dbPath = Path.Combine(exePath, dbName);
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        public static void EnsureDbCreated()
        {
            using var context = new AppDbContext();
            context.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura chave prim�ria de User.
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // Configura��o de Username (�nico e obrigat�rio)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(20);

            // Relacionamento entre User e UserTask (1:n)
            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}