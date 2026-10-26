using cecursos.Data;
using CeCursos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CeCursos.Controllers;

[ApiController]
[Route("api/cursos")]
public class CoursesController(CecursosDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCourses(
        [FromQuery] string? busca,
        [FromQuery] string? categoria,
        [FromQuery] string? modalidade,
        [FromQuery] string? status)
    {
        IQueryable<CourseViewModel> query =
            context.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = busca.Trim().ToLower();
            query = query.Where(c =>
                c.FullName.ToLower().Contains(termo) ||
                c.ShortName.ToLower().Contains(termo));
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            var valor = categoria.Trim().ToLower();
            query = query.Where(c => c.Category.Name.ToLower() == valor);
        }

        if (!string.IsNullOrWhiteSpace(modalidade))
        {
            var valor = modalidade.Trim().ToLower();
            query = query.Where(c => c.Modality.ToLower() == valor);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var valor = status.Trim().ToLower();
            query = query.Where(c => c.Status.ToLower() == valor);
        }

        var cursos = await query
            .Include(course => course.Category)
            .ToListAsync();

        return Ok(new
        {
            total = cursos.Count,
            resultados = cursos.Select(ToApiCourse)
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var curso = await context.Courses
            .AsNoTracking()
            .Include(course => course.Category)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (curso is null)
        {
            return NotFound(new
            {
                mensagem = $"Curso com ID {id} não foi encontrado."
            });
        }

        return Ok(ToApiCourse(curso));
    }

    private static object ToApiCourse(CourseViewModel course) => new
    {
        course.Id,
        course.FullName,
        course.ShortName,
        course.Description,
        course.Modules,
        course.LessonDescriptions,
        Category = course.Category.Name,
        IconImage = course.Category.IconImage,
        CategoryChipColor = course.Category.ChipColor,
        course.Modality,
        course.Workload,
        course.TargetAudience,
        course.Status,
        course.PresentationImage,
        course.AvailabilityDate
    };
}