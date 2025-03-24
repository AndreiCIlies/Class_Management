﻿using ClassManagementWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Class> Classes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurare relație Grade - Student
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configurare relație Grade - Class
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Class)
            .WithMany(c => c.Grades)
            .HasForeignKey(g => g.ClassId);
           // .OnDelete(DeleteBehavior.Cascade); 

        // Configurare relație Class - Teacher
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Classes)
            .HasForeignKey(c => c.TeacherId);

        // Relație Many-to-Many Student - Class
        modelBuilder.Entity<Class>()
            .HasMany(c => c.Students)
            .WithMany(s => s.Classes)
            .UsingEntity(j =>
            {
                j.ToTable("ClassStudent");
                j.Property<int>("ClassId").IsRequired();
                j.Property<string>("StudentId").IsRequired().HasMaxLength(450);
                j.HasKey("ClassId", "StudentId");

                // Ştergere în cascadă doar de la Class
                j.HasOne(typeof(Class)).WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.Cascade);

                // NU şterge în cascadă de la Student (Important!)
                j.HasOne(typeof(Student)).WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.NoAction);
            });

        // Configure inheritance for Student and Teacher
        modelBuilder.Entity<Student>().ToTable("Students");
        modelBuilder.Entity<Teacher>().ToTable("Teachers");
    }
}