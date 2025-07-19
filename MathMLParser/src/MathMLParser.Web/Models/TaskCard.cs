namespace MathMLParser.Web.Models;

public class TaskCard
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public string? CardColor { get; set; }
    public List<string>? Tags { get; set; }
    public int? ProgressPercentage { get; set; }
    public string? Notes { get; set; }

    // Foreign keys
    public int TaskId { get; set; }

    // Navigation properties
    public virtual Task Task { get; set; } = null!;
}