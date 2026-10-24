using System.Diagnostics;
using cecursos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CeCursos.Models;

namespace CeCursos.Controllers;

public class HomeController(CecursosDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var courses = await context.Courses
            .AsNoTracking()
            .OrderBy(course => course.Id)
            .ToListAsync();

        var testimonials = new[]
        {
            new CourseTestimonialViewModel
            {
                Quote = "A experiência do curso foi incrível! As aulas foram muito bem estruturadas e o cronograma foi cuidadosamente planejado, facilitando muito o aprendizado.",
                Name = "Lucas Pereira",
                Role = "Analista na Empresa X"
            },
            new CourseTestimonialViewModel
            {
                Quote = "Concluir o curso foi uma jornada enriquecedora. As aulas eram dinâmicas e o horário foi organizado para maximizar nosso tempo de estudo.",
                Name = "Mariana Costa",
                Role = "Gestora na Empresa X"
            },
            new CourseTestimonialViewModel
            {
                Quote = "O curso superou minhas expectativas! As aulas eram bem elaboradas e o planejamento do horário ajudou a manter o foco e a produtividade.",
                Name = "Thiago Almeida",
                Role = "Técnico na Empresa X"
            },
            new CourseTestimonialViewModel
            {
                Quote = "Finalizar o curso foi uma experiência motivadora. As aulas foram muito bem preparadas e o cronograma foi ideal para o aprendizado.",
                Name = "Fernanda Silva",
                Role = "Analista na Empresa X"
            },
            new CourseTestimonialViewModel
            {
                Quote = "A conclusão do curso foi uma etapa muito gratificante. As aulas eram bem organizadas e o horário foi pensado para facilitar nosso aprendizado.",
                Name = "Gabriel Santos",
                Role = "Analista na Empresa X"
            }
        };

        return View(new LandingPageViewModel
        {
            FeaturedCourses = courses,
            Testimonials = testimonials
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
