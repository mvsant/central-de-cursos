using CeCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace cecursos.Data;

public class CecursosDbContext(DbContextOptions<CecursosDbContext> options)
    : DbContext(options)
{
    public DbSet<CourseViewModel> Courses => Set<CourseViewModel>();
    public DbSet<CourseCategory> Categories => Set<CourseCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseCategory>()
            .HasIndex(category => category.Name)
            .IsUnique();

        modelBuilder.Entity<CourseViewModel>()
            .HasOne(course => course.Category)
            .WithMany()
            .HasForeignKey(course => course.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}