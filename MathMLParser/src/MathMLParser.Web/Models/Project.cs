namespace MathMLParser.Web.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public int? ProjectTemplateId { get; set; }
    public Dictionary<string, object>? Settings { get; set; }

    // Navigation properties
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    public virtual ProjectTemplate? ProjectTemplate { get; set; }
}