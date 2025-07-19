namespace MathMLParser.Web.Models
{
    public class TaskTemplate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int DefaultPriority { get; set; }
        public int EstimatedDurationHours { get; set; }
        public List<string>? RequiredEquations { get; set; }
        public List<string>? DefaultTags { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsPublic { get; set; }
        public string? Instructions { get; set; }
    }
}