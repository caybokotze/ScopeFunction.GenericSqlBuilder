namespace ScopeFunction.GenericSqlBuilder;

public class UpdateWhereCondition : Statement, IBuildable
{
    private readonly IUpdateOptions _options;

    public UpdateWhereCondition(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        _options = options;
    }

    public UpdateWhereCondition Where(string clause, bool applyPrefix = false)
    {
        if (_options is not UpdateOptions updateOptions)
        {
            throw new InvalidCastException(Errors.UpdateOptionCastException);
        }
        
        AddStatement($"WHERE {clause} ");
        return this;
    }
    
    public string Build()
    {
        return BuildStatement();
    }
}