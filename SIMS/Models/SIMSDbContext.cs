using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIMS.Models
{
    public partial class SIMSDbContext : DbContext
    {
        public SIMSDbContext()
        {
        }

        public SIMSDbContext(DbContextOptions<SIMSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-UQHR23R;Database=SIMS;User Id=sa;Password=123;TrustServerCertificate=True");
        }


    }


}
