using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder.Common;

public static class Helpers
{
    public static string GetPrefix(Options rootOptions, ISelectOptions options)
    {
        if (options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        if (rootOptions is {IgnorePrefix: true, Prefix: not null})
        {
            throw new InvalidStatementException(Errors.PrefixAndNoPrefixNotAllowed);
        }
        
        if (rootOptions.Prefix is not null)
        {
            return rootOptions.Prefix;
        }

        if (rootOptions.IgnorePrefix || selectOptions.IgnorePrefix)
        {
            return string.Empty;
        }

        return selectOptions.Prefix ?? string.Empty;
    }
}

