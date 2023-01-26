using System.Runtime.CompilerServices;

namespace ScopeFunction.GenericSqlBuilder;

public class UpdateStatement : Statement
{
    public UpdateStatement(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        
    }

    public UpdateWhereCondition Set(string clause)
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }

    public UpdateWhereCondition Set(string[] properties)
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }

    public UpdateWhereCondition Set(string[] properties, Action<IUpdateStatementBuilder> options)
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }

    public UpdateWhereCondition Set<T>(Func<T, string[]> properties) where T : class, new()
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }

    public UpdateWhereCondition Set<T>(Func<T, string[]> properties, Action<IUpdateOptions> options) where T : class, new()
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }
    
    public UpdateWhereCondition Set<T>(Action<IUpdateOptions> options) where T : class, new()
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }

    public UpdateWhereCondition Set<T>() where T : class, new()
    {
        return new UpdateWhereCondition(this, new UpdateOptions());
    }
}

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
        throw new NotImplementedException();
    }
}