namespace CeCursos.Models;

public class LandingPageViewModel
{
    public IReadOnlyList<CourseViewModel> FeaturedCourses { get; init; } = [];

    public IReadOnlyList<CourseTestimonialViewModel> Testimonials { get; init; } = [];
}

public class CourseTestimonialViewModel
{
    public required string Quote { get; init; }
    public required string Name { get; init; }
    public required string Role { get; init; }
}
