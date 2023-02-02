using System.Runtime.CompilerServices;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public class UpdateStatement : Statement
{
    private readonly IUpdateOptions _options;

    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement, options)
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
    public UpdateSetStatement Set(string[] properties)
    {
        return new UpdateSetStatement(this, _options);
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
    /// Supports an array with type assisting. If options were provided they will be applied.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    public UpdateSetStatement Set(Func<T, string[]> properties)
    {
        if (_options is not UpdateOptions uo)
        {
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        AddStatement("SET ");
        foreach (var segment in properties.Invoke(new T()))
        {
            AddStatement($"{GetPropertyVariant(ConvertCase(segment, uo.PropertyCase), uo.Variant)} = @{segment} ");
        }
        
        return new UpdateSetStatement(this, _options);
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
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        AddStatement("SET ");
        var segments = StatementBuilder.GetUpdateProperties<T>(uo);

        foreach (var segment in segments)
        {
            AddStatement(
                $"{GetPropertyVariant(ConvertCase(segment, uo.PropertyCase), uo.Variant)} = @{segment}, ");
        }
        
        TrimLast(true);
        AddStatement(" ");
        
        return new UpdateSetStatement(this, _options);
    }
}