# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a MathML equation parser AI agent built with ASP.NET Core 8.0 and Blazor Server. The system parses mathematical equations in MathML v3 format and creates dynamic web-based calculation interfaces for structural engineering applications.

## Development Commands

### Build and Run
```bash
# Build the solution
dotnet build

# Run the web application
dotnet run --project MathMLParser/src/MathMLParser.Web

# Run in watch mode for development
dotnet watch --project MathMLParser/src/MathMLParser.Web
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test MathMLParser/src/MathMLParser.Tests
```

### Database Operations
```bash
# Add new migration
dotnet ef migrations add MigrationName --project MathMLParser/src/MathMLParser.Web

# Update database
dotnet ef database update --project MathMLParser/src/MathMLParser.Web

# Drop database
dotnet ef database drop --project MathMLParser/src/MathMLParser.Web
```

### Code Quality
```bash
# Format code
dotnet format

# Restore packages
dotnet restore
```

## Architecture Overview

### Project Structure
- **MathMLParser.Web**: Main Blazor Server application
  - `Components/`: Reusable Blazor components for UI elements
  - `Services/`: Business logic services for parsing, calculations, and data management
  - `Models/`: Data models for equations, projects, tasks, and calculations
  - `Data/`: Entity Framework DbContext and migrations
  - `Pages/`: Blazor page components for different application views
  - `wwwroot/`: Static web assets including TeMML library for MathML rendering

- **MathMLParser.Tests**: Unit and integration tests

### Core Components

#### MathML Processing Pipeline
1. **MathMLParserService**: Parses MathML v3 XML and extracts variables
2. **ValidationService**: Validates input equations and user data
3. **CalculationService**: Evaluates mathematical expressions
4. **UnitService**: Handles engineering units (kN, mm, MPa, kNm) and dimensional analysis

#### Data Management
1. **Project Management**: Hierarchical structure (Projects → Tasks → Calculation Cards)
2. **Template System**: Reusable task configurations for common engineering scenarios
3. **Equation Library**: Persistent storage and categorization of equations
4. **ApplicationDbContext**: Entity Framework data context with SQLite for development

#### UI Architecture
- **Blazor Server Components**: Interactive server-side rendering
- **TeMML Integration**: Mathematical notation rendering in browsers
- **Responsive Design**: Card-based layout optimized for engineering workflows

### Key Services

- **MathMLParserService**: Core parsing logic for extracting variables from MathML
- **CalculationService**: Mathematical expression evaluation with unit handling
- **ProjectService**: Project lifecycle management and persistence
- **TaskService**: Task organization and template application
- **EquationLibraryService**: Equation storage, search, and categorization

### Data Models

Key entities include:
- `ParsedEquation`: Represents a parsed MathML equation with variables
- `Project`: Top-level container for engineering analysis
- `Task`: Grouped calculations for specific analysis scenarios
- `TaskCard`: Individual calculation instances with input data
- `Variable`: Represents mathematical variables with units and metadata

## Development Guidelines

### Working with MathML
- All equations must be valid MathML v3 2nd edition format
- Focus on engineering-specific variables and units
- Support subscripts, superscripts, and mathematical functions

### Engineering Units
The system supports these primary units:
- `kN` (kilonewtons) - Force
- `mm` (millimeters) - Length  
- `MPa` (megapascals) - Pressure/Stress
- `kNm` (kilonewton-meters) - Moment

### Database Development
- Use Entity Framework migrations for schema changes
- SQLite for development, designed for PostgreSQL/SQL Server in production
- Maintain referential integrity between Projects → Tasks → TaskCards

### Component Development
- Follow Blazor Server patterns for interactive components
- Implement real-time validation for user inputs
- Use TeMML for rendering mathematical notation
- Design for responsive, card-based layouts

## Testing Strategy

### Test Categories
1. **Parser Tests**: MathML parsing accuracy and variable extraction
2. **Calculation Tests**: Mathematical expression evaluation and unit conversion
3. **Integration Tests**: End-to-end workflows and data persistence
4. **UI Tests**: Component behavior and user interaction flows

### Test Data
Use engineering equations for realistic test scenarios:
- Moment capacity calculations
- Shear capacity analysis
- Deflection computations
- Connection design equations

## Technology Stack

- **Backend**: ASP.NET Core 8.0 with Blazor Server
- **Database**: Entity Framework Core with SQLite (development)
- **Frontend**: Blazor components with TeMML for math rendering
- **Testing**: xUnit framework with Moq for mocking
- **Development**: Docker dev container with pre-configured environment

## Common Workflows

### Adding New Equation Types
1. Update `EquationCategory` enum if needed
2. Implement parsing logic in `MathMLParserService`
3. Add unit tests for the new equation format
4. Update UI components for new variable types

### Creating New Templates
1. Define task structure in `TaskTemplate` model
2. Implement template creation in `TemplateService`
3. Add UI components for template management
4. Include validation for template data

### Extending Unit Support
1. Update `UnitService` with new unit definitions
2. Implement dimensional analysis rules
3. Add unit conversion logic
4. Update validation to support new units

This codebase focuses on structural engineering calculations with a hierarchical project management approach, emphasizing accuracy in mathematical computations and engineering unit handling.