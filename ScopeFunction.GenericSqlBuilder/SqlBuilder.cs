namespace ScopeFunction.GenericSqlBuilder;

public class SqlBuilder
{
    private readonly Statement? _statement;
    private readonly ISelectOptions? _selectOptions;

    public SqlBuilder(Statement statement, ISelectOptions selectOptions)
    {
        _statement = statement;
        _selectOptions = selectOptions;
    }

    public SqlBuilder()
    {
        
    }
    
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
        return _selectOptions is not null 
            ? new SelectStatement(new Statement(" "), _selectOptions) 
            : new SelectStatement(new Statement($"SELECT {clause} "), new SelectOptions());
    }
    
    public SelectStatement Select(string[] properties)
    {
        return new SelectStatement(new Statement("SELECT "), new SelectOptions
        {
            AppendAfterFrom = properties.ToList()
        });
    }
    
    public SelectStatement Select(string[] properties, Action<ISelectOptions> options)
    {
        var selectOptions = new SelectOptions();
        options(selectOptions);
        
        var sql = "SELECT ";
        if (selectOptions.TopValue is not null)
        {
            sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
        }
        
        selectOptions.AppendAfterFrom = properties.ToList();
        return new SelectStatement(new Statement(sql), selectOptions);
    }

    public SelectStatement Select<T>(Func<T, string[]> properties) where T : class, new()
    {
        return new SelectStatement(new Statement("SELECT "), new SelectOptions
        {
            AppendAfterFrom = properties(new T()).ToList()
        });
    }
    
    public SelectStatement Select<T>(Func<T, string[]> properties, Action<ISelectOptions> options) where T : class, new()
    {
        var selectOptions = new SelectOptions();
        options(selectOptions);

        selectOptions.AppendAfterFrom = properties(new T()).ToList();
        
        var sql = "SELECT ";
        if (selectOptions.TopValue is not null)
        {
            sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
        }
        
        return new SelectStatement(new Statement(sql), selectOptions);
    }
    
    public SelectStatement Select<T>() where T : class, new ()
    {
        var properties = StatementBuilder.GetSelectProperties<T>();
        var selectOptions = new SelectOptions
        {
            AppendAfterFrom = properties
        };
        
        return  new SelectStatement(new Statement("SELECT "), selectOptions);
    }

    public SelectStatement Select<T>(Action<ISelectOptions> options) where T : new()
    {
        var selectOptions = new SelectOptions();
        options(selectOptions);
        
        var sql = "SELECT ";
        
        if (selectOptions.TopValue is not null)
        {
            sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
        }
        
        var properties = StatementBuilder.GetSelectProperties<T>(selectOptions);
        selectOptions.AppendAfterFrom = properties;

        if (_selectOptions is not SelectOptions so)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }

        // append settings from previous builder
        if (so.IsAppendSelect)
        {
            selectOptions.IsAppendSelect = true;
        }
        
        return _selectOptions is not null 
            ? new SelectStatement(new Statement(""), selectOptions) 
            : new SelectStatement(new Statement(sql), selectOptions);
    }
}