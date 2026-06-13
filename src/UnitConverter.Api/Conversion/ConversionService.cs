using UnitsNet;
using UnitsNet.Units;

namespace UnitConverter.Api.Conversion;

// Conversion using UnitsNet
public class ConversionService
{
    public (double?, string toUnit) Convert(string category, string fromUnit, string toUnit, double? value)
    {
        if (value == null)
            return (null, toUnit);
        return category.ToLowerInvariant() switch
        {
            "length" => (ConvertLength(category, fromUnit, toUnit, value.Value), toUnit),
            "mass" => (ConvertMass(category, fromUnit, toUnit, value.Value), toUnit),
            "temperature" => (ConvertTemperature(category, fromUnit, toUnit, value.Value), toUnit),
            _ => throw new ArgumentException($"Unsupported category: {category}")
        };
    }

    // In /Validation/AllowedUnits.cs already defined and validated the allowed units for safe using Enum.Parse
    private double ConvertLength(string category, string fromUnit, string toUnit, double value)
    {
        LengthUnit from = (LengthUnit)UnitsDictionary.ParseUnit(category, fromUnit);
        LengthUnit to = (LengthUnit)UnitsDictionary.ParseUnit(category, toUnit);

        Length length = Length.From(value, from);

        return Math.Round(length.As(to), 6);
    }

    private double ConvertMass(string category, string fromUnit, string toUnit, double value)
    {
        MassUnit from = (MassUnit)UnitsDictionary.ParseUnit(category, fromUnit);
        MassUnit to = (MassUnit)UnitsDictionary.ParseUnit(category, toUnit);

        Mass mass = Mass.From(value, from);

        return Math.Round(mass.As(to), 4);
    }

    private double ConvertTemperature(string category, string fromUnit, string toUnit, double value)
    {
        TemperatureUnit from = (TemperatureUnit)UnitsDictionary.ParseUnit(category, fromUnit);
        TemperatureUnit to = (TemperatureUnit)UnitsDictionary.ParseUnit(category, toUnit);

        Temperature temperature = Temperature.From(value, from);

        return Math.Round(temperature.As(to), 1);
    }
}
