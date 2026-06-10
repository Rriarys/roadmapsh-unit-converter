namespace UnitConverter.Api.Validation;

public class AllowedUnits
{
    public static readonly Dictionary<string, List<string>> Categories = new()
    {
        ["length"] = ["millimeter", "centimeter", "meter", "kilometer", "inch", "foot", "yard", "mile"],
        ["weight"] = ["milligram", "gram", "kilogram", "ounce", "pound"],
        ["temperature"] = ["celsius", "fahrenheit", "kelvin"]
    };
}




// NOTE: Avaliable categories and units for conversion
//const categories = {
//    length: [
//        "millimeter",
//        "centimeter",
//        "meter",
//        "kilometer",
//        "inch",
//        "foot",
//        "yard",
//        "mile"
//    ],
//    weight: [
//        "milligram",
//        "gram",
//        "kilogram",
//        "ounce",
//        "pound"
//    ],
//    temperature: [
//        "celsius",
//        "fahrenheit",
//        "kelvin"
//    ]
//};