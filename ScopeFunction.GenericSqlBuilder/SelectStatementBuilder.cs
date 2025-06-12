namespace ScopeFunction.GenericSqlBuilder;

public interface ISelectStatementBuilder
{
    SelectStatement Select(string[] properties);
    SelectStatement Select(string[] properties, Action<ISelectOptions> options);
    SelectStatement Select<T>(Func<T, string[]> properties) where T : class, new();
    SelectStatement Select<T>(Func<T, string[]> properties, Action<ISelectOptions> options) where T : class, new();
    SelectStatement Select<T>() where T : class, new ();
    SelectStatement Select<T>(Action<ISelectOptions> options) where T : class, new();
}

public class SelectStatementBuilder : Statement, ISelectStatementBuilder
{
    private readonly ISelectOptions _options;

    public SelectStatementBuilder(Statement statement, ISelectOptions options) : base(statement)
    {
        _options = options;
    }
    
    public SelectStatementBuilder() : base(string.Empty, StatementType.Select)
    {
        _options = new SelectOptions();
    }

    public SelectStatement Select(string[] properties)
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        if (!so.IsAppendSelect)
        {
            AddStatement("SELECT ");
            return new SelectStatement(this, new SelectOptions
            {
                AppendAfterFromStatement = new List<AppendableAfterFrom>
                {
                    new(properties)
                }
            });
        }

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties, so.Prefix));
        return new SelectStatement(this, so);
    }
    
    public SelectStatement Select(string[] properties, Action<ISelectOptions> options)
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        if (!so.IsAppendSelect)
        {
            var selectOptions = new SelectOptions();
            options(selectOptions);
            
            var sql = "SELECT ";
            if (selectOptions.TopValue is not null)
            {
                sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
            }
        
            selectOptions.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties, selectOptions.Prefix));
            AddStatement(sql);
            
            return new SelectStatement(this, selectOptions);
        }
        
        options(_options);

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties, so.Prefix));

        return new SelectStatement(this, so);
    }

    public SelectStatement Select<T>(Func<T, string[]> properties) where T : class, new()
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        if (!so.IsAppendSelect)
        {
            AddStatement("SELECT ");

            var propList = properties(new T());
            
            return new SelectStatement(this, new SelectOptions
            {
                AppendAfterFromStatement = new List<AppendableAfterFrom>
                {
                    new(propList)
                }
            });
        }

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties(new T()), so.Prefix));
        return new SelectStatement(this, so);
    }
    
    public SelectStatement Select<T>(Func<T, string[]> properties, Action<ISelectOptions> options) where T : class, new()
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        if (!so.IsAppendSelect)
        {
            var selectOptions = new SelectOptions();
            options(selectOptions);

            selectOptions.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties(new T()), selectOptions.Prefix));
        
            var sql = "SELECT ";
            if (selectOptions.TopValue is not null)
            {
                sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
            }
            
            AddStatement(sql);
            return new SelectStatement(this, selectOptions);
        }

        options(_options);

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties(new T()), so.Prefix));
        return new SelectStatement(this, so);
    }
    
    public SelectStatement Select<T>() where T : class, new ()
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        var properties = StatementBuilder.GetPropertyNames<T>();
        
        if (!so.IsAppendSelect)
        {
            var selectOptions = new SelectOptions
            {
                AppendAfterFromStatement = new List<AppendableAfterFrom>
                {
                    new(properties.ToArray())
                }
            };
        
            AddStatement("SELECT ");
            return new SelectStatement(this, selectOptions);
        }

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(properties.ToArray(), so.Prefix));
        return new SelectStatement(this, so);
    }

    public SelectStatement Select<T>(Action<ISelectOptions> options) where T : class, new()
    {
        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.SelectOptionCastException);
        }
        
        if (!so.IsAppendSelect)
        {
            var selectOptions = new SelectOptions();
            options(selectOptions);
        
            var sql = "SELECT ";
        
            if (selectOptions.TopValue is not null)
            {
                sql = $"SELECT TOP {selectOptions.TopValue.ToString()} ";
            }

            AddStatement(sql);

            selectOptions
                .AppendAfterFromStatement
                .Add(new AppendableAfterFrom(StatementBuilder.GetSelectProperties<T>(selectOptions).ToArray(),
                    selectOptions.Prefix));
            
            return new SelectStatement(this, selectOptions);
        }

        options(_options);

        so.AppendAfterFromStatement.Add(new AppendableAfterFrom(StatementBuilder.GetSelectProperties<T>(so).ToArray(), so.Prefix));
        return new SelectStatement(this, so);
    }
}