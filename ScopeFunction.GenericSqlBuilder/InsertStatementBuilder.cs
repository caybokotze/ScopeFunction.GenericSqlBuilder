using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IInsertStatementBuilder
{
    InsertStatement Insert(string[] properties);
    InsertStatement Insert(string[] properties, Action<IInsertOptions> options);
    InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new();
    InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions> options) where T : class, new();
    InsertStatement Insert<T>() where T : class, new ();
    InsertStatement Insert<T>(Action<IInsertOptions> options) where T : class, new();
}

public class InsertStatementBuilder : Statement, IInsertStatementBuilder
{
    private readonly IInsertOptions _options;
    
    public InsertStatementBuilder(Statement statement, IInsertOptions options) : base(statement, options)
    {
        _options = options;
    }

    public InsertStatementBuilder() : base(new Statement("INSERT "), new InsertOptions())
    {
        _options = new InsertOptions();
    }

    public InsertStatement Insert(params string[] properties)
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert(string[] properties, Action<IInsertOptions> options)
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions> options) where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>() where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Action<IInsertOptions> options) where T : class, new()
    {
        throw new NotImplementedException();
    }
}