namespace UnitConverter.Api.Models;

public class ConvertRequest
{
    // category, fromUnit, toUnit, and value
    public string Category { get; set; } = null!;
    public string FromUnit { get; set; } = null!;
    public string ToUnit { get; set; } = null!;
    public double Value { get; set; }
}
