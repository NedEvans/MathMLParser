namespace MathMLParser.Web.Models;

public class Variable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public VariableType Type { get; set; }
    public double? Exponent { get; set; }
    public string? Unit { get; set; }
    public double? DefaultValue { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; } = true;
}

public enum VariableType
{
    Input,
    Output,
    Function,
    Constant
}