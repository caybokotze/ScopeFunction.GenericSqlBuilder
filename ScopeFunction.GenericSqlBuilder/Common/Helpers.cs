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

        if (whereOptions is {IgnorePrefix: true, Prefix: not null})
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

