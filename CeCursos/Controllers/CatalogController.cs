using cecursos.Data;
using CeCursos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CeCursos.Controllers;

[Route("cursos")]
public class CatalogController(CecursosDbContext context) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(
        [FromQuery] string? busca,
        [FromQuery] string? categoria,
        [FromQuery] string? modalidade,
        [FromQuery] string? status)
    {
        IQueryable<CourseViewModel> query = context.Courses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var term = busca.Trim().ToLower();
            query = query.Where(course =>
                course.FullName.ToLower().Contains(term) ||
                course.ShortName.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(categoria))
        {
            var category = categoria.Trim().ToLower();
            query = query.Where(course => course.Category.ToLower() == category);
        }

        if (!string.IsNullOrWhiteSpace(modalidade))
        {
            var modality = modalidade.Trim().ToLower();
            query = query.Where(course => course.Modality.ToLower() == modality);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            var courseStatus = status.Trim().ToLower();
            query = query.Where(course => course.Status.ToLower() == courseStatus);
        }

        var model = new CourseCatalogViewModel
        {
            Courses = await query.OrderBy(course => course.FullName).ToListAsync(),
            Categories = await context.Courses
                .AsNoTracking()
                .Select(course => course.Category)
                .Distinct()
                .OrderBy(categoryName => categoryName)
                .ToListAsync(),
            Modalities = await context.Courses
                .AsNoTracking()
                .Select(course => course.Modality)
                .Distinct()
                .OrderBy(modalityName => modalityName)
                .ToListAsync(),
            Statuses = await context.Courses
                .AsNoTracking()
                .Select(course => course.Status)
                .Distinct()
                .OrderBy(statusName => statusName)
                .ToListAsync(),
            Search = busca,
            Category = categoria,
            Modality = modalidade,
            Status = status
        };

        return View(model);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var course = await context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return course is null ? NotFound() : View(course);
    }

    [HttpGet("{id:int}/ava")]
    public async Task<IActionResult> Ava(int id)
    {
        var course = await context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (course is null)
        {
            return NotFound();
        }

        var lessonDescriptions = course.LessonDescriptions
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var sections = new List<CourseAvaSectionViewModel>
        {
            new()
            {
                Title = "Introdução",
                Topics =
                [
                    new CourseAvaTopicViewModel
                    {
                        Title = "Boas-vindas",
                        Content = course.Description
                    }
                ]
            }
        };

        var modules = course.Modules
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (var index = 0; index < modules.Length; index++)
        {
            var content = index < lessonDescriptions.Length
                ? lessonDescriptions[index]
                : "O conteúdo deste módulo será disponibilizado em breve.";
            var separator = content.IndexOf(':');
            var topicTitle = separator > 0
                ? content[..separator].Trim()
                : "Conteúdo do módulo";
            var topicContent = separator > 0
                ? content[(separator + 1)..].Trim()
                : content;

            sections.Add(new CourseAvaSectionViewModel
            {
                Title = modules[index],
                Topics =
                [
                    new CourseAvaTopicViewModel
                    {
                        Title = topicTitle,
                        Content = topicContent
                    }
                ]
            });
        }

        if (lessonDescriptions.Length > modules.Length)
        {
            sections.Add(new CourseAvaSectionViewModel
            {
                Title = "Materiais complementares",
                Topics = lessonDescriptions
                    .Skip(modules.Length)
                    .Select(description =>
                    {
                        var separator = description.IndexOf(':');
                        return new CourseAvaTopicViewModel
                        {
                            Title = separator > 0
                                ? description[..separator].Trim()
                                : "Material complementar",
                            Content = separator > 0
                                ? description[(separator + 1)..].Trim()
                                : description
                        };
                    })
                    .ToList()
            });
        }

        return View(new CourseAvaViewModel
        {
            CourseId = course.Id,
            CourseTitle = course.FullName,
            Sections = sections
        });
    }
}
