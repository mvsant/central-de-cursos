using CeCursos.Models;
using Microsoft.EntityFrameworkCore;

namespace cecursos.Data;

public class CecursosDbContext(DbContextOptions<CecursosDbContext> options)
    : DbContext(options)
{
    public DbSet<CourseViewModel> Courses => Set<CourseViewModel>();
}