namespace ScopeFunction.GenericSqlBuilder;

public class JoinStatement : Statement
{
    private readonly ISelectOptions _options;

    public JoinStatement(Statement statement, ISelectOptions options) : base(statement)
    {
        _options = options;
    }
    
    public FromStatement On(string clause)
    {
        AddStatement($"ON {clause} " );
        return new FromStatement(this, _options);
    }
}