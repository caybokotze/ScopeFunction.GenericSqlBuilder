namespace ScopeFunction.GenericSqlBuilder;

public class UpdateSetStatement : Statement
{
    private readonly IUpdateOptions _options;

    public UpdateSetStatement(Statement statement, IUpdateOptions options) : base(statement)
    {
        _options = options;
    }
    
    /// <summary>
    /// Will append the statement verbatim for cases which are not supported
    /// </summary>
    /// <param name="clause"></param>
    public Finalise Append(string clause)
    {
        AddStatement($"{clause} ");
        return new Finalise(this, _options);
    }
    
    public UpdateWhereStatement Where(string clause, bool applyPrefix = false)
    {
        if (_options is not UpdateOptions updateOptions)
        {
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        AddStatement($"WHERE {clause} ");
        return new UpdateWhereStatement(this, _options);
    }
    
    public UpdateWhereStatement Where(string clause, Action<UpdateWhereCondition> condition)
    {
        var whereCondition = new UpdateWhereCondition(this, _options);
        
        if (_options is not UpdateOptions)
        {
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        // todo: apply some prefix / case conversion
        
        AddStatement($"WHERE {clause} ");
        
        condition(whereCondition);
        
        return new UpdateWhereStatement(this, _options);
    }
}