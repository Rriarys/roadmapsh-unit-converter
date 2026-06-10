using UnitConverter.Api.Models;

namespace UnitConverter.Api.Validation;

public static class ConvertRequestValidator
{
    /// <summary>
    /// Validates the conversion request category and units against allowed values.
    /// </summary>
    /// <param name="request">The conversion request to validate.</param>
    /// <returns>A tuple containing a boolean indicating validity and an error message if validation fails.</returns>
    public static (bool IsValid, string Error) Validate(ConvertRequest request)
    {
        var checks = new[]
        {
            Check(request.Category, nameof(request.Category)),
            Check(request.FromUnit, nameof(request.FromUnit)),
            Check(request.ToUnit, nameof(request.ToUnit)),
            Check(request.Value, nameof(request.Value))
        };

        foreach (var check in checks)
        {
            if (!check.ok)
                return (false, check.error!);
        }

        if (!AllowedUnits.Categories.ContainsKey(request.Category)) // Keys ignore case sensitivity
        {
            return (false, $"Invalid category. Allowed categories are: {string.Join(", ", AllowedUnits.Categories.Keys)}");
        }

        var units = AllowedUnits.Categories[request.Category];

        if (!units.Contains(request.FromUnit.ToLowerInvariant()))
        {
            return (false, $"Invalid from units. Allowed units for {request.Category} are: {string.Join(", ", units)}");
        }

        if (!units.Contains(request.ToUnit.ToLowerInvariant()))
        {
            return (false, $"Invalid to units. Allowed units for {request.Category} are: {string.Join(", ", units)}");
        }

        return (true, string.Empty);
    }

    /// <summary>
    /// Validates that a value is not null, empty, or whitespace.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="fieldName">The name of the field to include in the error message if validation fails.</param>
    /// <returns>A tuple containing a boolean indicating whether validation passed and an optional error message.</returns>
    private static (bool ok, string? error) Check(string? value, string fieldName)
    {
        return string.IsNullOrWhiteSpace(value)
            ? (false, $"{fieldName} is required")
            : (true, null);
    }
    // Overload
    /// <summary>
    /// Validates a nullable double value (overload of Check for numeric fields)
    /// </summary>
    /// <param name="value">The nullable double value to validate.</param>
    /// <param name="name">The name of the parameter being validated, used in error messages.</param>
    /// <returns>A tuple where ok indicates whether the value is valid, and error contains the validation error message if
    /// validation failed, or null if validation succeeded.</returns>
    private static (bool ok, string? error) Check(double? value, string name)
    {
        if (!value.HasValue)
            return (false, $"{name} is required");

        if (double.IsNaN(value.Value))
            return (false, $"{name} cannot be NaN");

        if (double.IsInfinity(value.Value))
            return (false, $"{name} cannot be Infinity");

        return (true, null);
    }
}
