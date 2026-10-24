namespace CeCursos.Models;

public class CourseCatalogViewModel
{
    public IReadOnlyList<CourseViewModel> Courses { get; init; } = [];
    public IReadOnlyList<string> Categories { get; init; } = [];
    public IReadOnlyList<string> Modalities { get; init; } = [];
    public IReadOnlyList<string> Statuses { get; init; } = [];
    public string? Search { get; init; }
    public string? Category { get; init; }
    public string? Modality { get; init; }
    public string? Status { get; init; }
}
