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
            query = query.Where(c => c.Category.ToLower() == valor);
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

        var cursos = await query.ToListAsync();

        return Ok(new
        {
            total = cursos.Count,
            resultados = cursos
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var curso = await context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (curso is null)
        {
            return NotFound(new
            {
                mensagem = $"Curso com ID {id} não foi encontrado."
            });
        }

        return Ok(curso);
    }
}