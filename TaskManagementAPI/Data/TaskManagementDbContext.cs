using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity
                    .HasOne(e => e.User)
                    .WithMany(e => e.Projects)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Task configuration
            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Priority).HasConversion<int>();

                entity
                    .HasOne(e => e.Project)
                    .WithMany(e => e.Tasks)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.User)
                    .WithMany(e => e.Tasks)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TaskHistory configuration
            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Comment).IsRequired().HasMaxLength(2000);

                entity
                    .HasOne(e => e.Task)
                    .WithMany(e => e.TaskHistories)
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity
                    .HasOne(e => e.User)
                    .WithMany(e => e.TaskHistories)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
