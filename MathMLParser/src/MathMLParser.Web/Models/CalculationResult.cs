namespace MathMLParser.Web.Models;

public class CalculationResult
{
    public int Id { get; set; }
    public double? Value { get; set; }
    public string? Unit { get; set; }
    public bool IsValid { get; set; } = true;
    public string? ErrorMessage { get; set; }
    public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object>? Variables { get; set; }

    // Foreign keys
    public int? EquationId { get; set; }

    // Navigation properties
    public virtual ParsedEquation? ParsedEquation { get; set; }
}