using ScopeFunction.GenericSqlBuilder.Enums;
using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder.Common;

public class VariantHelpers
{
    public string GetVariantLastInsertedId(Variant variant, string? key = null)
    {
        return variant switch
        {
            Variant.Default => "LAST_INSERT_ID() ",
            Variant.CosmosDb => throw new InvalidStatementException(Errors.CosmosDbLastInsertNotSupported),
            Variant.MsSql => "SCOPE_IDENTITY() ",
            Variant.PostgreSql => $"RETURNING {key} ",
            Variant.MySql => "LAST_INSERT_ID() ",
            _ => "LAST_INSERT_ID() "
        };
    }

    public static string GetPropertyVariant(string segment, Variant variant)
    {
        return variant switch
        {
            Variant.MySql => $"`{segment}`",
            Variant.MsSql => $"[{segment}]",
            _ => segment
        };
    }

    public static string GetTableVariant(string segment, Variant variant)
    {
        return variant switch
        {
            _ => segment
        };
    }
}