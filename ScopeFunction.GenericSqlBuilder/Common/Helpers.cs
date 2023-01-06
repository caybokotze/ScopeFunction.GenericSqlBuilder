using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder.Common;

public static class Helpers
{
    public static string GetPrefix(Options whereOptions, ISelectOptions options)
    {
        if (options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        if (whereOptions.IgnorePrefix && whereOptions.Prefix is not null)
        {
            throw new InvalidStatementException(Errors.PrefixAndNoPrefixNotAllowed);
        }
        
        if (whereOptions.Prefix is not null)
        {
            return whereOptions.Prefix;
        }

        if (whereOptions.IgnorePrefix || selectOptions.IgnorePrefix)
        {
            return string.Empty;
        }

        return selectOptions.Prefix ?? string.Empty;
    }
}

public class Options
{
    public bool IgnorePrefix { get; set; }
    public string? Prefix { get; set; }
}

