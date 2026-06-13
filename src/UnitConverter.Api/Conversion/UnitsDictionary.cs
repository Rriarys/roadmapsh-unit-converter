using UnitsNet;
using UnitsNet.Units;

namespace UnitConverter.Api.Conversion;

//UnitsNet
public static class UnitsDictionary
{
    // Method wich return Enum.Parse wich depends on the category and unit name, using the dictionaries defined below
    public static Enum ParseUnit(string category, string unitName)
    {
        // Input data is already validated in /Validation/ConvertRequestValidator.cs, so we can safely use the dictionaries below
        return category.ToLowerInvariant() switch
        {
            "length" => LengthUnits[unitName],
            "mass" => MassUnits[unitName],
            "temperature" => TemperatureUnits[unitName],
            _ => throw new InvalidOperationException($"Unknown category '{category}'")
        };
    }

    private static readonly Dictionary<string, LengthUnit> LengthUnits =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["millimeter"] = LengthUnit.Millimeter,
        ["centimeter"] = LengthUnit.Centimeter,
        ["meter"] = LengthUnit.Meter,
        ["kilometer"] = LengthUnit.Kilometer,
        ["inch"] = LengthUnit.Inch,
        ["foot"] = LengthUnit.Foot,
        ["yard"] = LengthUnit.Yard,
        ["mile"] = LengthUnit.Mile
    };

    private static readonly Dictionary<string, MassUnit> MassUnits =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["milligram"] = MassUnit.Milligram,
        ["gram"] = MassUnit.Gram,
        ["kilogram"] = MassUnit.Kilogram,
        ["ounce"] = MassUnit.Ounce,
        ["pound"] = MassUnit.Pound
    };

    private static readonly Dictionary<string, TemperatureUnit> TemperatureUnits =
    new(StringComparer.OrdinalIgnoreCase)
    {
        ["celsius"] = TemperatureUnit.DegreeCelsius,
        ["fahrenheit"] = TemperatureUnit.DegreeFahrenheit,
        ["kelvin"] = TemperatureUnit.Kelvin
    };
}
