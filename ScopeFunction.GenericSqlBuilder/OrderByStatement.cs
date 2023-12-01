namespace ScopeFunction.GenericSqlBuilder;

public class OrderByStatement : Statement
{
    private readonly ISelectOptions _options;

    public OrderByStatement(Statement statement, ISelectOptions options) : base(statement)
    {
        _options = options;
    }

    public OrderByStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
    
    public string Build()
    {
        return BuildStatement();
    }
}