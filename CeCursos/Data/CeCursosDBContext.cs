using CeCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace cecursos.Data;

public class CecursosDbContext(DbContextOptions<CecursosDbContext> options)
    : DbContext(options)
{
    // Add DbSet<TEntity> properties here, e.g.:
    public DbSet<CourseViewModel> Courses => Set<CourseViewModel>();
}