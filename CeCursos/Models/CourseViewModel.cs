namespace CeCursos.Models;

public class CourseViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;       // Nome completo
        public string ShortName { get; set; } = string.Empty;      // Nome breve
        public string Description { get; set; } = string.Empty;    // Descrição
        public string Category { get; set; } = string.Empty;       // Categoria (Min 3 total in DB)
        public string Modality { get; set; } = string.Empty;       // Modalidade (Min 2 total in DB)
        public int Workload { get; set; }                          // Carga horária
        public string TargetAudience { get; set; } = string.Empty; // Público-alvo
        public string Status { get; set; } = string.Empty;         // Status (Min 3 total in DB)
        public string PresentationImage { get; set; } = string.Empty; // Imagem de apresentação
        public DateTime? AvailabilityDate { get; set; }            // Data de disponibilização
    }
