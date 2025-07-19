namespace MathMLParser.Web.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string>? Errors { get; set; }
        public List<string>? Warnings { get; set; }
        public string? ValidatedContent { get; set; }
        public DateTime ValidatedAt { get; set; }
        public string? ValidationType { get; set; }
        public Dictionary<string, object>? ValidationDetails { get; set; }
        public string? Suggestions { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
    }
}