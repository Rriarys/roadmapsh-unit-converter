namespace UnitConverter.Api.Models;

public class ConvertResponse
{
    //result, toUnit
    public double? Result { get; set; }
    public string ToUnit { get; set; } = null!;
}
