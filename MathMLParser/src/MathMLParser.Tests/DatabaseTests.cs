using Microsoft.EntityFrameworkCore;
using MathMLParser.Web.Data;
using MathMLParser.Web.Models;
using Xunit;

namespace MathMLParser.Tests;

public class DatabaseTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public DatabaseTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async System.Threading.Tasks.Task CanCreateProject_WithTasks_AndTaskCards()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Description = "A test project for database verification",
            CreatedBy = "Test User",
            Status = "Active"
        };

        var task = new MathMLParser.Web.Models.Task
        {
            Name = "Test Task",
            Description = "A test task",
            Status = "Pending",
            Priority = "High",
            Project = project
        };

        var taskCard = new TaskCard
        {
            Title = "Test Task Card",
            Description = "A test task card",
            Status = "Todo",
            Priority = "Medium",
            Task = task
        };

        // Act
        _context.Projects.Add(project);
        _context.Tasks.Add(task);
        _context.TaskCards.Add(taskCard);
        await _context.SaveChangesAsync();

        // Assert
        var savedProject = await _context.Projects
            .Include(p => p.Tasks)
            .ThenInclude(t => t.TaskCards)
            .FirstOrDefaultAsync(p => p.Name == "Test Project");

        Assert.NotNull(savedProject);
        Assert.Equal("Test Project", savedProject.Name);
        Assert.Single(savedProject.Tasks);
        Assert.Equal("Test Task", savedProject.Tasks.First().Name);
        Assert.Single(savedProject.Tasks.First().TaskCards);
        Assert.Equal("Test Task Card", savedProject.Tasks.First().TaskCards.First().Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task CanCreateEquation_WithVariables()
    {
        // Arrange
        var equation = new ParsedEquation
        {
            Name = "Test Equation",
            Description = "A test equation",
            MathMLContent = "<math><mi>F</mi><mo>=</mo><mi>m</mi><mi>a</mi></math>",
            Category = "Physics",
            IsValid = true
        };

        var variables = new List<Variable>
        {
            new Variable
            {
                Name = "F",
                DisplayName = "F",
                Type = VariableType.Output,
                Unit = "N",
                Description = "Force"
            },
            new Variable
            {
                Name = "m",
                DisplayName = "m",
                Type = VariableType.Input,
                Unit = "kg",
                Description = "Mass"
            },
            new Variable
            {
                Name = "a",
                DisplayName = "a",
                Type = VariableType.Input,
                Unit = "m/s²",
                Description = "Acceleration"
            }
        };

        equation.Variables = variables;

        // Act
        _context.ParsedEquations.Add(equation);
        await _context.SaveChangesAsync();

        // Assert
        var savedEquation = await _context.ParsedEquations
            .Include(e => e.Variables)
            .FirstOrDefaultAsync(e => e.Name == "Test Equation");

        Assert.NotNull(savedEquation);
        Assert.Equal("Test Equation", savedEquation.Name);
        Assert.Equal(3, savedEquation.Variables.Count);
        Assert.Contains(savedEquation.Variables, v => v.Name == "F" && v.Type == VariableType.Output);
        Assert.Contains(savedEquation.Variables, v => v.Name == "m" && v.Type == VariableType.Input);
        Assert.Contains(savedEquation.Variables, v => v.Name == "a" && v.Type == VariableType.Input);
    }

    [Fact]
    public async System.Threading.Tasks.Task CanCreateCalculationResult_WithJsonSerialization()
    {
        // Arrange
        var equation = new ParsedEquation
        {
            Name = "Test Equation",
            MathMLContent = "<math><mi>y</mi><mo>=</mo><mi>x</mi></math>",
            Category = "Math"
        };

        var calculationResult = new CalculationResult
        {
            Value = 42.5,
            Unit = "kN",
            IsValid = true,
            Variables = new Dictionary<string, object>
            {
                { "x", 10.0 },
                { "y", 42.5 },
                { "unit_x", "m" },
                { "unit_y", "kN" }
            },
            ParsedEquation = equation
        };

        // Act
        _context.ParsedEquations.Add(equation);
        _context.CalculationResults.Add(calculationResult);
        await _context.SaveChangesAsync();

        // Assert
        var savedResult = await _context.CalculationResults
            .Include(cr => cr.ParsedEquation)
            .FirstOrDefaultAsync(cr => cr.Value == 42.5);

        Assert.NotNull(savedResult);
        Assert.Equal(42.5, savedResult.Value);
        Assert.Equal("kN", savedResult.Unit);
        Assert.True(savedResult.IsValid);
        Assert.NotNull(savedResult.Variables);
        Assert.Equal(4, savedResult.Variables.Count);
        // In-memory database preserves object types, SQLite serializes to JSON
        var xValue = savedResult.Variables["x"];
        if (xValue is System.Text.Json.JsonElement jsonElement)
        {
            Assert.Equal(10.0, jsonElement.GetDouble());
        }
        else
        {
            Assert.Equal(10.0, Convert.ToDouble(xValue));
        }
        Assert.NotNull(savedResult.ParsedEquation);
        Assert.Equal("Test Equation", savedResult.ParsedEquation.Name);
    }

    [Fact]
    public async System.Threading.Tasks.Task CanCreateProjectTemplate_WithDefaultSettings()
    {
        // Arrange
        var template = new ProjectTemplate
        {
            Name = "Steel Analysis Template",
            Description = "Template for steel structure analysis",
            Category = "Structural",
            CreatedBy = "Engineer",
            IsPublic = true,
            DefaultSettings = new Dictionary<string, object>
            {
                { "units", "SI" },
                { "safety_factor", 1.5 },
                { "code", "AISC" }
            }
        };

        // Act
        _context.ProjectTemplates.Add(template);
        await _context.SaveChangesAsync();

        // Assert
        var savedTemplate = await _context.ProjectTemplates
            .FirstOrDefaultAsync(pt => pt.Name == "Steel Analysis Template");

        Assert.NotNull(savedTemplate);
        Assert.Equal("Steel Analysis Template", savedTemplate.Name);
        Assert.True(savedTemplate.IsPublic);
        Assert.NotNull(savedTemplate.DefaultSettings);
        Assert.Equal(3, savedTemplate.DefaultSettings.Count);
        // In-memory database preserves object types, SQLite serializes to JSON
        var unitsValue = savedTemplate.DefaultSettings["units"];
        if (unitsValue is System.Text.Json.JsonElement jsonElement)
        {
            Assert.Equal("SI", jsonElement.GetString());
        }
        else
        {
            Assert.Equal("SI", unitsValue.ToString());
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}