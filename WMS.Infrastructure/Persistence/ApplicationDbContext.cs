using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;
using WMS.Infrastructure.Identity;

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace WMS.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<UserLogin> UserLogins => Set<UserLogin>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<EmployeeProjectAllocation> EmployeeProjectAllocations => Set<EmployeeProjectAllocation>();
        public DbSet<Announcement> Announcements => Set<Announcement>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleName)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            // Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.Property(e => e.DepartmentName)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // UserLogin
            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(e => e.Username)
                      .IsUnique();
                
                entity.Property(e => e.PasswordHash)
                      .IsRequired();

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Employee)
                      .WithMany()
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Employee
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.LastName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(80);

                entity.HasIndex(e => e.Email)
                      .IsUnique();

                entity.Property(e => e.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.Gender)
                      .IsRequired()
                      .HasMaxLength(1);

                entity.Property(e => e.Status)
                      .HasMaxLength(20)
                      .HasDefaultValue("Active");

                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Role)
                      .WithMany(r => r.Employees)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Attendance configuration
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.AttendanceId);

                entity.Property(e => e.WorkMode)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.Attendances)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // leave configuration
            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.LeaveRequestId);

                entity.Property(e => e.Reason)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.LeaveType)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.ApprovedBy)
                      .HasMaxLength(50);

                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.LeaveRequests)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // client configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.Property(e => e.ClientName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.ClientAddress);

                entity.Property(e => e.ClientPhoneNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.ClientLocation)
                      .HasMaxLength(20);
            });

            // project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.ProjectName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Status)
                      .HasMaxLength(20);

                entity.HasOne(e => e.Client)
                      .WithMany(c => c.Projects)
                      .HasForeignKey(e => e.ClientId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //allocation configuration
            modelBuilder.Entity<EmployeeProjectAllocation>(entity =>
            {
                entity.HasKey(e => e.AllocationId);

                entity.Property(e => e.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(50);

                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.ProjectAllocations)
                      .HasForeignKey(e => e.EmpId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Allocations)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // announcement configuration
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.HasKey(e => e.AnnouncementId);
                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.Message)
                      .IsRequired();
                entity.Property(e => e.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            // audit log configuration
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(e => e.AuditId);
            });

            // Seed initial data
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    RoleName = "Admin",
                    Description = "System Administrator"
                },
                new Role
                {
                    RoleId = 2,
                    RoleName = "Employee",
                    Description = "Regular Employee"
                }
            );

            modelBuilder.Entity<UserLogin>().HasData(
                new UserLogin
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = PasswordHasher.HashPassword("Admin@123"),
                    RoleId = 1,
                    LastLogin = null,
                    EmployeeId = null
                }
            );
        }
    }
}
