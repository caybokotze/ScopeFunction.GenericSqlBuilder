using System.Runtime.CompilerServices;
using ScopeFunction.GenericSqlBuilder.Common;

namespace ScopeFunction.GenericSqlBuilder;

public class UpdateStatement : Statement
{
    private readonly IUpdateOptions _options;

    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        _options = options;
    }
    
    /// <summary>
    /// Verbatim Set. No specified options will be set.
    /// </summary>
    /// <param name="clause"></param>
    /// <returns></returns>
    public UpdateWhereCondition Set(string clause)
    {
        AddStatement($"SET {clause} ");
        return new UpdateWhereCondition(this, _options);
    }

    /// <summary>
    /// Supports an array of properties. If any options were provided, they will be applied.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public UpdateWhereCondition Set(string[] properties)
    {
        return new UpdateWhereCondition(this, _options);
    }
}


public class UpdateStatement<T> : Statement where T : new()
{
    private readonly IUpdateOptions _options;

    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        _options = options;
    }

    
    /// <summary>
    /// Supports an array with type assisting. Will apply options if they were provided
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public UpdateWhereCondition Set(Func<T, string[]> properties)
    {
        AddStatement("SET ");
        foreach (var segment in properties.Invoke(new T()))
        {
            AddStatement($"{segment} = @{segment} ");
        }
        
        return new UpdateWhereCondition(this, _options);
    }

    public UpdateWhereCondition Set()
    {
        if (_options is not UpdateOptions uo)
        {
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        AddStatement("SET ");
        var segments = StatementBuilder.GetUpdateProperties<T>(uo);

        foreach (var segment in segments)
        {
            AddStatement(
                $"{VariantHelpers.GetPropertyVariant(CaseConverter.ConvertCase(segment, uo.PropertyCase), uo.Variant)} = @{segment}, ");
        }
        
        TrimLast(true);
        AddStatement(" ");
        
        return new UpdateWhereCondition(this, _options);
    }
}