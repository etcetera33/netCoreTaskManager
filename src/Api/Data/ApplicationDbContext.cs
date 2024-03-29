﻿using Core.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemAudit> WorkItemAudits { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<WorkItemFile> WorkItemFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkItem>(e =>
            {
                e.Property(wi => wi.WorkItemType).HasConversion(x => (int)x, x => (WorkItemTypes)x);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.Property(u => u.Role).HasConversion(x => (int)x, x => (Roles)x);
            });

            modelBuilder.Entity<WorkItem>(e =>
                {
                    e.HasOne(x => x.Assignee).WithMany(x => x.AssignedTo).HasForeignKey(x => x.AssigneeId).OnDelete(DeleteBehavior.Restrict);
                    e.HasOne(x => x.Author).WithMany(x => x.CreatedWorkItems).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Restrict);
                }
            );

            modelBuilder.Entity<Project>(e =>
            {
                e.HasOne(x => x.Owner).WithMany(x => x.Projects).HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>()
               .HasIndex(u => u.Email)
               .IsUnique(false);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.ExternalId)
                .IsUnique();

            modelBuilder.Entity<WorkItemAudit>()
               .HasIndex(u => u.WorkItemId);

            modelBuilder.Entity<WorkItemFile>()
                .HasKey(bc => new { bc.WorkItemFileId });

            modelBuilder.Entity<WorkItemFile>()
                .HasOne(bc => bc.File)
                .WithMany(b => b.WorkItemFiles)
                .HasForeignKey(bc => bc.FileId);

            modelBuilder.Entity<WorkItemFile>()
                .HasOne(bc => bc.WorkItem)
                .WithMany(c => c.WorkItemFiles)
                .HasForeignKey(bc => bc.WorkItemId);
        }
    }
}
