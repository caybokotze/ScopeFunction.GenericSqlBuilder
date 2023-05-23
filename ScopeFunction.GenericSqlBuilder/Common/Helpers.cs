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

    public static string GetPrefix(ISelectOptions selectOptions, string? table = null)
    {
        if (selectOptions is not SelectOptions so)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }

        return so switch
        {
            {IgnorePrefix: true, Prefix: not null} => throw new InvalidStatementException(Errors.PrefixAndNoPrefixNotAllowed),
            {IgnorePrefix: false, Prefix: null} when table is not null => $"{table}.",
            _ => so.Prefix is not null ? $"{so.Prefix}." : string.Empty
        };
    }

    public static string GetPrefix(IUpdateOptions updateOptions)
    {
        if (updateOptions is not UpdateOptions uo)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        if (uo is {IgnorePrefix: true, Prefix: not null})
        {
            throw new InvalidStatementException(Errors.PrefixAndNoPrefixNotAllowed);
        }

        return uo.Prefix is not null ? $"{uo.Prefix}." : string.Empty;
    }
}

