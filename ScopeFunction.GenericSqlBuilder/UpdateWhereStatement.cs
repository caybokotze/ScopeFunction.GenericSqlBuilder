namespace ScopeFunction.GenericSqlBuilder;

public class UpdateWhereStatement : Statement, IBuildable
{
    private readonly IUpdateOptions _options;

    public UpdateWhereStatement(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        _options = options;
    }

    public UpdateWhereStatement And(string clause)
    {
        AddStatement($"AND {clause} ");
        return this;
    }
    
    public UpdateWhereStatement And(string clause, Action<UpdateWhereCondition> condition)
    {
        var whereCondition = new UpdateWhereCondition(this, _options);
        
        AddStatement($"AND {clause} ");
        condition(whereCondition);
        return this;
    }
    
    public UpdateWhereStatement Or(string clause)
    {
        AddStatement($"OR {clause} ");
        return this;
    }
    
    public UpdateWhereStatement Or(string clause, Action<UpdateWhereCondition> condition)
    {
        var whereCondition = new UpdateWhereCondition(this, _options);
        
        AddStatement($"OR {clause} ");
        
        condition(whereCondition);
        return this;
    }

    public Finalise Append(string clause)
    {
        AddStatement($"{clause} ");
        return new Finalise(this, _options);
    }
    
    public string Build()
    {
        return BuildStatement();
    }
}