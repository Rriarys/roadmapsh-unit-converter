namespace UnitConverter.Api.Validation;

public class AllowedUnits
{
    public static readonly Dictionary<string, List<string>> Categories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["length"] = ["millimeter", "centimeter", "meter", "kilometer", "inch", "foot", "yard", "mile"],
        ["mass"] = ["milligram", "gram", "kilogram", "ounce", "pound"],
        ["temperature"] = ["celsius", "fahrenheit", "kelvin"]
    };
}