using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public class UpdateStatement : Statement
{
    private readonly IUpdateOptions _options;

    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement)
    {
        _options = options;
    }

    /// <summary>
    /// Verbatim Set. If options were provided they will not take effect.
    /// </summary>
    /// <param name="clause"></param>
    /// <returns></returns>
    public UpdateSetStatement Set(string clause)
    {
        AddStatement($"SET {clause} ");
        return new UpdateSetStatement(this, _options);
    }

    /// <summary>
    /// Supports an array of properties. If options were provided they will be applied.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public UpdateSetStatement Set(IEnumerable<string> properties)
    {
        if (_options is not UpdateOptions uo)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.UpdateOptionCastException);
        }

        AddStatement("SET ");

        foreach (var segment in properties)
        {
            var columnName = GetColumnName(uo.SourceType, segment, uo.PropertyCase);
            AddStatement($"{GetPropertyVariant(columnName, uo.Variant)} = @{segment}");
            AddStatement(", ");
        }

        RemoveLast();
        AddStatement(" ");

        return new UpdateSetStatement(this, _options);
    }

    private static string GetColumnName(Type? sourceType, string propertyName, Casing casing)
    {
        if (sourceType is null)
        {
            return ConvertCase(propertyName, casing);
        }

        return StatementBuilder.GetColumnName(sourceType, propertyName, casing);
    }
}


public class UpdateStatement<T> : Statement where T : new()
{
    private readonly IUpdateOptions _options;

    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement)
    {
        _options = options;
    }

    /// <summary>
    /// Will only build up properties reflectively. If options were provided they will be applied.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidCastException"></exception>
    public UpdateSetStatement Set()
    {
        if (_options is not UpdateOptions uo)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.UpdateOptionCastException);
        }

        AddStatement("SET ");

        var properties = StatementBuilder.GetUpdateProperties<T>(uo);

        foreach (var property in properties)
        {
            if (uo.RemovedProperties.Contains(property))
            {
                continue;
            }

            var columnName = StatementBuilder.GetColumnName<T>(property, uo.PropertyCase);
            AddStatement($"{GetPropertyVariant(columnName, uo.Variant)} = @{property}");
            AddStatement(", ");
        }

        RemoveLast();
        AddStatement(" ");

        return new UpdateSetStatement(this, _options);
    }
}