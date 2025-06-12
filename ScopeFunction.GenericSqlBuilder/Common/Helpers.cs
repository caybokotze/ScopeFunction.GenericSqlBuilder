using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder.Common;

public static class Helpers
{
    public static string GetPrefix(Options rootOptions, ISelectOptions options)
    {
        if (options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                SqlBuilderErrorConstants.SelectOptionCastException);
        }

        if (rootOptions is {IgnorePrefix: true, Prefix: not null})
        {
            throw new InvalidStatementException(SqlBuilderErrorConstants.PrefixAndNoPrefixNotAllowed);
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

    public static string GetPrefix(ISelectOptions selectOptions, string? table = null, string? prefix = null)
    {
        if (prefix is not null)
        {
            return $"{prefix}.";
        }
        
        if (selectOptions is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }

        return so switch
        {
            {IgnorePrefix: true, Prefix: not null} => throw new InvalidStatementException(SqlBuilderErrorConstants.PrefixAndNoPrefixNotAllowed),
            {IgnorePrefix: false, Prefix: null} when table is not null => $"{table}.",
            _ => so.Prefix is not null ? $"{so.Prefix}." : string.Empty
        };
    }

}

