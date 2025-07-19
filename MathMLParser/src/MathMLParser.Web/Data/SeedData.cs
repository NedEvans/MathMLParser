using MathMLParser.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace MathMLParser.Web.Data;

public static class SeedData
{
    public static async System.Threading.Tasks.Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists
        if (await context.Projects.AnyAsync() || await context.EquationLibrary.AnyAsync())
        {
            return; // Data already seeded
        }

        // Seed equations for structural engineering
        var equations = new List<EquationSummary>
        {
            new EquationSummary
            {
                Name = "Moment Capacity",
                Description = "Steel beam moment capacity calculation",
                Category = "MomentCapacity",
                MathMLContent = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
                    <mrow>
                        <msub><mi>M</mi><mi>n</mi></msub>
                        <mo>=</mo>
                        <msub><mi>f</mi><mi>y</mi></msub>
                        <mo>×</mo>
                        <msub><mi>S</mi><mi>x</mi></msub>
                    </mrow>
                </math>",
                Unit = "kNm",
                CreatedAt = DateTime.UtcNow,
                UsageCount = 0
            },
            new EquationSummary
            {
                Name = "Shear Capacity",
                Description = "Steel beam shear capacity calculation",
                Category = "ShearCapacity",
                MathMLContent = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
                    <mrow>
                        <msub><mi>V</mi><mi>n</mi></msub>
                        <mo>=</mo>
                        <mn>0.6</mn>
                        <mo>×</mo>
                        <msub><mi>f</mi><mi>y</mi></msub>
                        <mo>×</mo>
                        <msub><mi>A</mi><mi>w</mi></msub>
                    </mrow>
                </math>",
                Unit = "kN",
                CreatedAt = DateTime.UtcNow,
                UsageCount = 0
            },
            new EquationSummary
            {
                Name = "Deflection",
                Description = "Simply supported beam deflection",
                Category = "Deflection",
                MathMLContent = @"<math xmlns=""http://www.w3.org/1998/Math/MathML"">
                    <mrow>
                        <mi>δ</mi>
                        <mo>=</mo>
                        <mfrac>
                            <mrow><mn>5</mn><mi>w</mi><msup><mi>L</mi><mn>4</mn></msup></mrow>
                            <mrow><mn>384</mn><mi>E</mi><mi>I</mi></mrow>
                        </mfrac>
                    </mrow>
                </math>",
                Unit = "mm",
                CreatedAt = DateTime.UtcNow,
                UsageCount = 0
            }
        };

        context.EquationLibrary.AddRange(equations);

        // Seed a sample project template
        var projectTemplate = new ProjectTemplate
        {
            Name = "Steel Beam Analysis",
            Description = "Template for analyzing steel beam capacity and deflection",
            Category = "Structural",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            IsPublic = true,
            UsageCount = 0,
            DefaultSettings = new Dictionary<string, object>
            {
                { "DefaultUnits", "SI" },
                { "SafetyFactor", 1.5 },
                { "DesignCode", "AISC" }
            }
        };

        context.ProjectTemplates.Add(projectTemplate);

        // Seed task templates
        var taskTemplates = new List<TaskTemplate>
        {
            new TaskTemplate
            {
                Name = "Moment Capacity Check",
                Description = "Check moment capacity of steel beam",
                Category = "Capacity",
                DefaultPriority = 2,
                EstimatedDurationHours = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                IsPublic = true,
                Instructions = "Calculate the nominal moment capacity using yield stress and section modulus"
            },
            new TaskTemplate
            {
                Name = "Deflection Check",
                Description = "Check deflection limits for serviceability",
                Category = "Serviceability",
                DefaultPriority = 1,
                EstimatedDurationHours = 1,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System",
                IsPublic = true,
                Instructions = "Calculate maximum deflection and compare to allowable limits"
            }
        };

        context.TaskTemplates.AddRange(taskTemplates);

        // Seed sample variables for demonstration
        var variables = new List<Variable>
        {
            new Variable
            {
                Name = "fy",
                DisplayName = "f_y",
                Type = VariableType.Input,
                Unit = "MPa",
                Description = "Yield strength of steel",
                IsRequired = true,
                DefaultValue = 250.0
            },
            new Variable
            {
                Name = "Sx",
                DisplayName = "S_x",
                Type = VariableType.Input,
                Unit = "mm³",
                Description = "Section modulus about x-axis",
                IsRequired = true
            },
            new Variable
            {
                Name = "Mn",
                DisplayName = "M_n",
                Type = VariableType.Output,
                Unit = "kNm",
                Description = "Nominal moment capacity",
                IsRequired = false
            }
        };

        context.Variables.AddRange(variables);

        // Save all changes
        await context.SaveChangesAsync();
    }
}