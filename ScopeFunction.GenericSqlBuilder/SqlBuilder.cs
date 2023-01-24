namespace ScopeFunction.GenericSqlBuilder;

public class SqlBuilder : ISelectStatementBuilder, IInsertStatementBuilder
{
    public SqlBuilder()
    {
        
    }

    #region INSERT STATEMENTS
    public InsertStatement Insert(params string[] properties)
    {
        return new InsertStatementBuilder().Insert(properties);
    }

    public InsertStatement Insert(string[] properties, Action<IInsertOptions> options)
    {
        return new InsertStatementBuilder().Insert(properties, options);
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new()
    {
        return new InsertStatementBuilder().Insert(properties);
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions> options) where T : class, new()
    {
        return new InsertStatementBuilder().Insert(properties, options);
    }

    public InsertStatement Insert<T>() where T : class, new()
    {
        return new InsertStatementBuilder().Insert<T>();
    }

    public InsertStatement Insert<T>(Action<IInsertOptions> options) where T : class, new()
    {
        return new InsertStatementBuilder().Insert<T>(options);
    }
    #endregion
    

    # region SELECT STATEMENTS
    
    public SelectStatement SelectAll()
    {
        return new SelectStatement(new Statement("SELECT * "), new SelectOptions());
    }
    
    public SelectStatement SelectAll(Action<ISelectOptions> options)
    {
        var selectOptions = new SelectOptions();
        options(selectOptions);
        return new SelectStatement(new Statement("SELECT * "), selectOptions);
    }

    public SelectStatement Select(string clause)
    {
        return new SelectStatement(new Statement($"SELECT {clause}"), new SelectOptions());
    }

    public SelectStatement Select(string[] properties)
    {
        return new SelectStatementBuilder().Select(properties);
    }

    public SelectStatement Select(string[] properties, Action<ISelectOptions> options)
    {
        return new SelectStatementBuilder().Select(properties, options);
    }

    public SelectStatement Select<T>(Func<T, string[]> properties) where T : class, new()
    {
        return new SelectStatementBuilder().Select(properties);
    }

    public SelectStatement Select<T>(Func<T, string[]> properties, Action<ISelectOptions> options) where T : class, new()
    {
        return new SelectStatementBuilder().Select(properties, options);
    }

    public SelectStatement Select<T>() where T : class, new()
    {
        return new SelectStatementBuilder().Select<T>();
    }

    public SelectStatement Select<T>(Action<ISelectOptions> options) where T : class, new()
    {
        return new SelectStatementBuilder().Select<T>(options);
    }
    # endregion
}

