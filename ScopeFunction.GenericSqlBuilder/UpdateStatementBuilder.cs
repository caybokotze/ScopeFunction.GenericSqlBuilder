namespace ScopeFunction.GenericSqlBuilder;

public interface IUpdateStatementBuilder
{
    UpdateStatement Update(string table);
}

public class UpdateStatementBuilder : Statement, IUpdateStatementBuilder
{
    public UpdateStatementBuilder() : base(new Statement(string.Empty), new UpdateOptions())
    {
        
    }
    
    public UpdateStatement Update(string table)
    {
        AddStatement($"{table} ");
        return new UpdateStatement(this, new UpdateOptions());
    }
}