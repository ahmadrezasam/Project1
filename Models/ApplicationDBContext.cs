using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Protocols;
using StudentManagementSystemCore.Models;

namespace StudentManagementSystemCore.Models
{
    public partial class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Tools> Tools { get; set; }
        public virtual DbSet<Platforms> Platform { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<ResourceLinks> ResourceLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tools>().HasOne<ResourceLinks>().WithOne(l => l.Tools).HasForeignKey<ResourceLinks>(p => p.ToolsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Student>().HasOne<Courses>().WithOne(s => s.Students).HasForeignKey<Courses>(c => c.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        
        public async Task<ResourceLinks> GetResourceLinkAsync(int id)
        {
            return await ResourceLinks
                .Include(l => l.Tools)
                .SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Tools> GetToolAsync(int id)
        {
            return await Tools.FindAsync(id);
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await Student.FindAsync(id);
        }

        public async Task<Courses> GetCourseAsync(int id)
        {
            return await Courses.Include(l => l.Students)
                .SingleOrDefaultAsync(l => l.Id == id);
        }

    }


}
