namespace MathMLParser.Web.Models;

public class ParsedEquation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MathMLContent { get; set; } = string.Empty;
    public string? LatexContent { get; set; }
    public string? ResultUnit { get; set; }
    public bool IsValid { get; set; } = true;
    public List<string>? ValidationErrors { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public virtual ICollection<Variable> Variables { get; set; } = new List<Variable>();
}