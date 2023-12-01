namespace ScopeFunction.GenericSqlBuilder;

public interface IUpdateStatementBuilder
{
    UpdateStatement Update(string table);
    UpdateStatement Update(string table, Action<IUpdateOptions> options);
    UpdateStatement<T> Update<T>(string table) where T : new();
    UpdateStatement<T> Update<T>(string table, Action<IUpdateOptions<T>> options) where T : new();
}

public class UpdateStatementBuilder : Statement, IUpdateStatementBuilder
{
    public UpdateStatementBuilder() : base(new Statement(string.Empty, StatementType.Update))
    {
        
    }
    
    public UpdateStatement Update(string table)
    {
        AddStatement($"UPDATE {table} ");
        return new UpdateStatement(this, new UpdateOptions());
    }
    
    public UpdateStatement Update(string table, Action<IUpdateOptions> options)
    {
        AddStatement($"UPDATE {table} ");
        
        var updateOptions = new UpdateOptions
        {
            Table = table
        };
        
        options(updateOptions);
        return new UpdateStatement(this, updateOptions);
    }
    
    public UpdateStatement<T> Update<T>(string table) where T : new()
    {
        AddStatement($"UPDATE {table} ");
        
        return new UpdateStatement<T>(this, new UpdateOptions
        {
            Table = table
        });
    }
    
    public UpdateStatement<T> Update<T>(string table, Action<IUpdateOptions<T>> options) where T : new()
    {
        AddStatement($"UPDATE {table} ");
        
        var updateOptions = new UpdateOptions<T>
        {
            Table = table
        };
        
        options(updateOptions);
        return new UpdateStatement<T>(this, updateOptions);
    }
}