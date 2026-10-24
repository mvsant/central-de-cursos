namespace CeCursos.Models;

public sealed class CourseAvaViewModel
{
    public int CourseId { get; init; }
    public string CourseTitle { get; init; } = string.Empty;
    public List<CourseAvaSectionViewModel> Sections { get; init; } = [];
}

public sealed class CourseAvaSectionViewModel
{
    public string Title { get; init; } = string.Empty;
    public List<CourseAvaTopicViewModel> Topics { get; init; } = [];
}

public sealed class CourseAvaTopicViewModel
{
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
}
