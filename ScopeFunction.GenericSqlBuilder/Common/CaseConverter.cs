using PeanutButter.Utils;

namespace ScopeFunction.GenericSqlBuilder.Common;

public static class CaseConverter
{
    public static string ConvertCase(string segment, Casing casing)
    {
        return casing switch
        {
            Casing.Default => segment,
            Casing.UpperCase => segment.ToUpperInvariant(),
            Casing.LowerCase => segment.ToLowerInvariant(),
            Casing.KebabCase => segment.ToKebabCase(),
            Casing.PascalCase => segment.ToPascalCase(),
            Casing.SnakeCase => segment.ToSnakeCase(),
            Casing.CamelCase => segment.ToCamelCase(),
            _ => segment
        };
    }
}

