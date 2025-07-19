using Microsoft.EntityFrameworkCore;
using MathMLParser.Web.Models;

namespace MathMLParser.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Core entities
    public DbSet<Project> Projects { get; set; }
    public DbSet<Models.Task> Tasks { get; set; }
    public DbSet<TaskCard> TaskCards { get; set; }
    public DbSet<ParsedEquation> ParsedEquations { get; set; }
    public DbSet<Variable> Variables { get; set; }
    public DbSet<CalculationResult> CalculationResults { get; set; }

    // Template entities
    public DbSet<ProjectTemplate> ProjectTemplates { get; set; }
    public DbSet<TaskTemplate> TaskTemplates { get; set; }

    // Library entities
    public DbSet<EquationSummary> EquationLibrary { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Project relationships
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.HasIndex(e => e.Name);
        });

        // Task relationships
        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedTo).HasMaxLength(100);

            entity.HasOne(t => t.Project)
                  .WithMany(p => p.Tasks)
                  .HasForeignKey(t => t.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ProjectId);
        });

        // TaskCard relationships
        modelBuilder.Entity<TaskCard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.AssignedTo).HasMaxLength(100);
            entity.Property(e => e.CardColor).HasMaxLength(20);

            entity.HasOne(tc => tc.Task)
                  .WithMany(t => t.TaskCards)
                  .HasForeignKey(tc => tc.TaskId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.TaskId);
        });

        // ParsedEquation relationships
        modelBuilder.Entity<ParsedEquation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.MathMLContent).IsRequired();
            entity.Property(e => e.LatexContent).HasMaxLength(2000);
            entity.Property(e => e.ResultUnit).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasMany(pe => pe.Variables)
                  .WithOne()
                  .HasForeignKey("ParsedEquationId")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
        });

        // Variable relationships
        modelBuilder.Entity<Variable>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // CalculationResult relationships
        modelBuilder.Entity<CalculationResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

            entity.HasOne(cr => cr.ParsedEquation)
                  .WithMany()
                  .HasForeignKey(cr => cr.EquationId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.EquationId);
            entity.HasIndex(e => e.CalculatedAt);
        });

        // ProjectTemplate relationships
        modelBuilder.Entity<ProjectTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
        });

        // TaskTemplate relationships
        modelBuilder.Entity<TaskTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.Instructions).HasMaxLength(2000);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
        });

        // EquationSummary relationships
        modelBuilder.Entity<EquationSummary>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MathMLContent).IsRequired();
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.LastUsedAt);
        });

        // Configure JSON columns for complex properties
        modelBuilder.Entity<Project>()
            .Property(e => e.Settings)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null!)
            );

        modelBuilder.Entity<CalculationResult>()
            .Property(e => e.Variables)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null!)
            );

        modelBuilder.Entity<ParsedEquation>()
            .Property(e => e.ValidationErrors)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null!)
            );

        modelBuilder.Entity<ProjectTemplate>()
            .Property(e => e.DefaultSettings)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(v, (System.Text.Json.JsonSerializerOptions)null!)
            );

        modelBuilder.Entity<TaskCard>()
            .Property(e => e.Tags)
            .HasConversion(
                v => v == null ? null : System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                v => v == null ? null : System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions)null!)
            );
    }
}