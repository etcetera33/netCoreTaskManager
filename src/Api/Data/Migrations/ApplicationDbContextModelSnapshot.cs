﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("CommentBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkItemId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("WorkItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Data.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProjectId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            ProjectId = 1,
                            ProjectName = "Apple",
                            Slug = "apple"
                        },
                        new
                        {
                            ProjectId = 2,
                            ProjectName = "Facebook",
                            Slug = "facebook"
                        });
                });

            modelBuilder.Entity("Data.Models.Status", b =>
                {
                    b.Property<int>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("StatusName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StatusId");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            StatusId = 1,
                            StatusName = "To do"
                        },
                        new
                        {
                            StatusId = 2,
                            StatusName = "Doing"
                        },
                        new
                        {
                            StatusId = 3,
                            StatusName = "To test"
                        },
                        new
                        {
                            StatusId = 4,
                            StatusName = "Testing"
                        },
                        new
                        {
                            StatusId = 5,
                            StatusName = "Done"
                        });
                });

            modelBuilder.Entity("Data.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasFilter("[Login] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            FullName = "Dmytro123213 Poliit",
                            Login = "dmyto123123123123.poliit",
                            Password = "111",
                            Position = "Junior Developer",
                            Role = 1,
                            RoleId = 1
                        },
                        new
                        {
                            UserId = 2,
                            FullName = "John123123 Doe",
                            Login = "john123123123.doe",
                            Password = "111",
                            Position = "Junior PM",
                            Role = 0,
                            RoleId = 0
                        });
                });

            modelBuilder.Entity("Data.Models.WorkItem", b =>
                {
                    b.Property<int>("WorkItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssigneeId")
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkItemType")
                        .HasColumnType("int");

                    b.Property<int>("WorkItemTypeId")
                        .HasColumnType("int");

                    b.HasKey("WorkItemId");

                    b.HasIndex("AssigneeId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("StatusId");

                    b.ToTable("WorkItems");

                    b.HasData(
                        new
                        {
                            WorkItemId = 1,
                            AssigneeId = 1,
                            AuthorId = 2,
                            Description = "Deploy the project",
                            ProjectId = 1,
                            StatusId = 1,
                            Title = "Deploy project",
                            WorkItemType = 1,
                            WorkItemTypeId = 1
                        });
                });

            modelBuilder.Entity("Data.Models.Comment", b =>
                {
                    b.HasOne("Data.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.WorkItem", "WorkItem")
                        .WithMany("Comments")
                        .HasForeignKey("WorkItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Models.WorkItem", b =>
                {
                    b.HasOne("Data.Models.User", "Assignee")
                        .WithMany("AssignedTo")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.Models.User", "Author")
                        .WithMany("CreatedWorkItems")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.Models.Project", "Project")
                        .WithMany("WorkItems")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
