namespace ScopeFunction.GenericSqlBuilder;

public class SqlBuilder : ISelectStatementBuilder, IInsertStatementBuilder, IUpdateStatementBuilder
{
    #region UPDATE STATEMENT
    public UpdateStatement Update(string table)
    {
        return new UpdateStatementBuilder().Update(table);
    }
    
    public UpdateStatement Update(string table, Action<IUpdateOptions> options)
    {
        return new UpdateStatementBuilder().Update(table, options);
    }

    public UpdateStatement<T> Update<T>(string table) where T : new()
    {
        return new UpdateStatementBuilder().Update<T>(table);
    }

    public UpdateStatement<T> Update<T>(string table, Action<IUpdateOptions> options) where T : new()
    {
        return new UpdateStatementBuilder().Update<T>(table, options);
    }

    #endregion
    
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

