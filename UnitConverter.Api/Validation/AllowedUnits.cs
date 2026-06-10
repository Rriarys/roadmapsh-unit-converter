namespace UnitConverter.Api.Validation;

public class AllowedUnits
{
    public static readonly Dictionary<string, List<string>> Categories = new Dictionary<string, List<string>>
    {
        {
            "length", new List<string>
            {
                "millimeter",
                "centimeter",
                "meter",
                "kilometer",
                "inch",
                "foot",
                "yard",
                "mile"
            }
        },
        {
            "weight", new List<string>
            {
                "milligram",
                "gram",
                "kilogram",
                "ounce",
                "pound"
            }
        },
        {
            "temperature", new List<string>
            {
                "celsius",
                "fahrenheit",
                "kelvin"
            }
        }
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