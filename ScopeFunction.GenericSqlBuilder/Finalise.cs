namespace ScopeFunction.GenericSqlBuilder;

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