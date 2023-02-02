namespace ScopeFunction.GenericSqlBuilder;

public class OrderByStatement : Statement
{
    private readonly ISelectOptions _options;

    public OrderByStatement(
        Statement statement, 
        ISelectOptions options) : base(statement, options)
    {
        _options = options;
    }

    public Finalise Desc()
    {
        AddStatement("DESC ");
        return new Finalise(this, _options);
    }

    public Finalise Asc()
    {
        AddStatement("ASC ");
        return new Finalise(this, _options);
    }

    public OrderByStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
}

public class Finalise : Statement
{
    public Finalise(Statement statement, ISelectOptions options) : base(statement, options)
    {
        
    }

    public Finalise(Statement statement, IInsertOptions options) : base(statement, options)
    {
        
    }

    public Finalise(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        
    }
    
    public string Build()
    {
        return BuildStatement();
    }
}