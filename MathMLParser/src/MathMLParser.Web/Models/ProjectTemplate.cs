namespace MathMLParser.Web.Models
{
    public class ProjectTemplate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public List<TaskTemplate>? TaskTemplates { get; set; }
        public List<EquationSummary>? DefaultEquations { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsPublic { get; set; }
        public int UsageCount { get; set; }
        public Dictionary<string, object>? DefaultSettings { get; set; }
    }
}