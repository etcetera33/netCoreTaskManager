using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>(project => project.HasIndex(x => x.Slug).IsUnique(true));

            modelBuilder.Entity<WorkItem>(e =>
                {
                    e.HasOne(x => x.Assignee).WithMany(x => x.AssignedTo).HasForeignKey(x => x.AssigneeId).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(x => x.Author).WithMany(x => x.CreatedWorkItems).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
                }
            );

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Spectator" },
                new Role { RoleId = 2, RoleName = "Developer" },
                new Role { RoleId = 3, RoleName = "Owner" }
            );
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = 1, StatusName = "To do" },
                new Status { StatusId = 2, StatusName = "Doing" },
                new Status { StatusId = 3, StatusName = "To test" },
                new Status { StatusId = 4, StatusName = "Testing" },
                new Status { StatusId = 5, StatusName = "Done" }
            );
            modelBuilder.Entity<Project>().HasData(
                new Project { ProjectId = 1, ProjectName = "Apple", Slug = "apple" },
                new Project { ProjectId = 2, ProjectName = "Facebook", Slug = "facebook" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Login = "dmyto.poliit", FullName = "Dmytro Poliit", Password = "111", Position = "Junior Developer" },
                new User { UserId = 2, Login = "john.doe", FullName = "John Doe", Password = "111", Position = "Junior PM" }
            );
            modelBuilder.Entity<WorkItem>().HasData(
                new WorkItem { WorkItemId = 1, ProjectId = 1, AssigneeId = 1, AuthorId = 2, Title = "Deploy project", Description = "Deploy the project", StatusId = 1 }
            );
        }
    }
}
