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

    public Statement Desc()
    {
        AddStatement("DESC ");
        return this;
    }

    public Statement Asc()
    {
        AddStatement("ASC ");
        return this;
    }

    public OrderByStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
}