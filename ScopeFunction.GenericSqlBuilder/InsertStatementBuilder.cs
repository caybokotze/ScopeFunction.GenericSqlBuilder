using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IInsertStatementBuilder
{
    InsertStatement Insert(string[] properties);
    InsertStatement Insert(string[] properties, Action<IInsertOptions> options);
    InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new();
    InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions<T>> options) where T : class, new();
    InsertStatement Insert<T>() where T : class, new ();
    InsertStatement Insert<T>(Action<IInsertOptions<T>> options) where T : class, new();
}

public class InsertStatementBuilder : Statement, IInsertStatementBuilder
{
    public InsertStatementBuilder() : base("INSERT ", StatementType.Insert)
    {
    }

    public InsertStatement Insert(params string[] properties)
    {
        var insertOptions = new InsertOptions();
        insertOptions.AppendAfterIntoStatement.AddRange(properties);
        return new InsertStatement(this, insertOptions);
    }

    public InsertStatement Insert(string[] properties, Action<IInsertOptions> options)
    {
        var insertOptions = new InsertOptions();
        options(insertOptions);
        
        insertOptions.AppendAfterIntoStatement.AddRange(properties);
        return new InsertStatement(this, insertOptions);
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new()
    {
        var insertOptions = new InsertOptions<T>();
        insertOptions.AppendAfterIntoStatement.AddRange(properties.Invoke(new T()));
        return new InsertStatement(this, insertOptions);
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions<T>> options) where T : class, new()
    {
        
        var insertOptions = new InsertOptions<T>();
        options(insertOptions);
        
        insertOptions.AppendAfterIntoStatement.AddRange(properties.Invoke(new T()));
        
        return new InsertStatement(this, insertOptions);
    }

    public InsertStatement Insert<T>() where T : class, new()
    {
        var properties = StatementBuilder.GetPropertyNames<T>();
        
        var insertOptions = new InsertOptions<T>();
        
        insertOptions.AppendAfterIntoStatement.AddRange(properties);
        
        return new InsertStatement(this, insertOptions);
    }

    public InsertStatement Insert<T>(Action<IInsertOptions<T>> options) where T : class, new()
    {
        var properties = StatementBuilder.GetPropertyNames<T>();
        
        var insertOptions = new InsertOptions<T>();
        options(insertOptions);
        insertOptions.AppendAfterIntoStatement.AddRange(properties);
        
        return new InsertStatement(this, insertOptions);
    }
}