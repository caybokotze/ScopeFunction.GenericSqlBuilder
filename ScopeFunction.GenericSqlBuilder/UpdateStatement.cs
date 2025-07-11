using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ScopeFunction.GenericSqlBuilder.Common;
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
            AddStatement($"{GetPropertyVariant(ConvertCase(segment, uo.PropertyCase), uo.Variant)} = @{segment}");
            AddStatement(", ");
        }
        
        RemoveLast();
        AddStatement(" ");
        
        return new UpdateSetStatement(this, _options);
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
            
            AddStatement($"{GetPropertyVariant(ConvertCase(property, uo.PropertyCase), uo.Variant)} = @{property}");
            AddStatement(", ");
        }
        
        RemoveLast();
        AddStatement(" ");
        
        return new UpdateSetStatement(this, _options);
    }
}