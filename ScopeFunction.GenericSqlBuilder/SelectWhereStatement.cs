using ScopeFunction.GenericSqlBuilder.Common;

namespace ScopeFunction.GenericSqlBuilder;


public class SelectWhereStatement : Statement, IBuildable
{
    private readonly ISelectOptions _options;
    
    public SelectWhereStatement(
        Statement statement, 
        ISelectOptions options) : base(statement, options)
    {
        _options = options;
    }

    public string Build()
    {
        return BuildStatement();
    }

    public FromStatement And()
    {
        AddStatement("AND ");
        
        if (_options is not SelectOptions options)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        options.IsAppendWhere = true;

        var whereOptions = new WhereOptions
        {
            HasAndSeparator = true
        };
        
        return new FromStatement(this, options, whereOptions);
    }
    
    public SelectWhereStatement And(string clause)
    {
        AddStatement($"AND {clause} ");
        return this;
    }

    public FromStatement Or()
    {
        AddStatement("OR ");
        
        if (_options is not SelectOptions options)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        options.IsAppendWhere = true;
        
        var whereOptions = new WhereOptions
        {
            HasAndSeparator = true
        };
        
        return new FromStatement(this, options, whereOptions);
    }
    
    public SelectWhereStatement Or(string clause)
    {
        AddStatement($"OR {clause} ");
        return this;
    }

    public SelectWhereStatement In(params string[] clause)
    {
        AddStatement($"IN ({string.Join(", ", clause)})");
        return this;
    }

    # region ORDER BY

    private void AddOrderByClauses(IReadOnlyCollection<string> clauses, OrderByOptions orderByOptions)
    {
        var prefix = Helpers.GetPrefix(orderByOptions, _options);
        var ascOrDesc = orderByOptions.IsDesc ? "DESC" : "ASC";
        
        AddStatement("ORDER BY ");

        var multipleClauses = clauses.Count > 1;
        var i = 0;
        
        foreach (var clause in clauses)
        {
            i++;
            var lastItem = i == clauses.Count;
            
            if (string.IsNullOrEmpty(prefix))
            {
                if (multipleClauses && !lastItem)
                {
                    AddStatement($"{clause} {ascOrDesc}, ");
                    continue;
                }
                
                AddStatement($"{clause} {ascOrDesc} ");
                continue;
            }

            if (multipleClauses && !lastItem)
            {
                AddStatement($"{prefix}.{clause} {ascOrDesc}, ");
                continue;
            }
            
            AddStatement($"{prefix}.{clause} {ascOrDesc} ");
        }
    }

    public OrderByStatement OrderBy(string clause)
    {
        AddStatement($"ORDER BY {clause} ");
        return new OrderByStatement(this, _options);
    }

    public OrderByStatement OrderBy(string clause, Action<IOrderByOptions> options)
    {
        var orderByOptions = new OrderByOptions();
        options(orderByOptions);
        
        var ascOrDesc = orderByOptions.IsDesc ? "DESC" : "ASC";

        AddStatement($"ORDER BY {clause} {ascOrDesc} ");
        return new OrderByStatement(this, _options);
    }

    public OrderByStatement OrderBy(string[] clauses)
    {
        var orderByOptions = new OrderByOptions();
        AddOrderByClauses(clauses, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    
    public OrderByStatement OrderBy(string[] clauses, Action<IOrderByOptions> options)
    {
        var orderByOptions = new OrderByOptions();
        options(orderByOptions);
        AddOrderByClauses(clauses, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    
    public OrderByStatement OrderBy<T>(Func<T, string> clause) where T : new()
    {
        var statement = clause(new T());
        var orderByOptions = new OrderByOptions();
        AddOrderByClauses(new []{ statement }, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    
    public OrderByStatement OrderBy<T>(Func<T, string> clause, Action<IOrderByOptions> options) where T : new()
    {
        var statement = clause(new T());
        var orderByOptions = new OrderByOptions();
        options(orderByOptions);
        
        AddOrderByClauses(new []{ statement }, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    
    public OrderByStatement OrderBy<T>(Func<T, string[]> clauses) where T : new()
    {
        var statements = clauses(new T());
        var orderByOptions = new OrderByOptions();
        AddOrderByClauses(statements, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    
    public OrderByStatement OrderBy<T>(Func<T, string[]> clauses, Action<IOrderByOptions> options) where T : new()
    {
        var statements = clauses(new T());
        var orderByOptions = new OrderByOptions();
        options(orderByOptions);
        AddOrderByClauses(statements, orderByOptions);
        return new OrderByStatement(this, _options);
    }
    # endregion

    public SelectWhereStatement AppendWhere(Action<FromStatement> statement)
    {
        if (_options is not SelectOptions options)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        options.IsAppendWhere = true;
        
        statement(new FromStatement(this, options));
        return this;
    }

    public SelectWhereStatement AppendWhereIf(Func<bool> condition, Action<FromStatement> statement)
    {
        var outcome = condition();
        if (!outcome)
        {
            return this;
        }
        
        if (_options is not SelectOptions options)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        options.IsAppendWhere = true;

        statement(new FromStatement(this, options));
        return this;
    }
    
    public SelectWhereStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
    
    public SelectWhereStatement AppendIf(Func<bool> condition, string clause)
    {
        var outcome = condition();

        if (!outcome)
        {
            return this;
        }
        
        AddStatement($"{clause} ");
        return this;
    }
}