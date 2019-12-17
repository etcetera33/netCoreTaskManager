using Core.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkItem>(e =>
            {
                e.Property(e => e.WorkItemType).HasConversion(x => (int)x, x => (WorkItemTypes)x);
            });
            modelBuilder.Entity<User>(e =>
            {
                e.Property(e => e.Role).HasConversion(x => (int)x, x => (Roles)x);
            });

            modelBuilder.Entity<WorkItem>(e =>
                {
                    e.HasOne(x => x.Assignee).WithMany(x => x.AssignedTo).HasForeignKey(x => x.AssigneeId).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(x => x.Author).WithMany(x => x.CreatedWorkItems).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
                }
            );

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<Project>().HasData(
                new Project { ProjectId = 1, ProjectName = "Apple"},
                new Project { ProjectId = 2, ProjectName = "Facebook"}
            );

            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Login = "dmyto123123123123.poliit", FullName = "Dmytro123213 Poliit", Password = "111", Position = "Junior Developer", RoleId = (int)Roles.Developer },
                new User { UserId = 2, Login = "john123123123.doe", FullName = "John123123 Doe", Password = "111", Position = "Junior PM", RoleId = (int)Roles.Spectator }
            );
            modelBuilder.Entity<WorkItem>().HasData(
                new WorkItem { WorkItemId = 1, ProjectId = 1, AssigneeId = 1, AuthorId = 2, Title = "Deploy project", Description = "Deploy the project", StatusId = 1, WorkItemTypeId = 1 }
            );
        }
    }
}
