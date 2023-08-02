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

    private void AddOrderByClauses(IReadOnlyCollection<string> clauses, Options orderByOptions)
    {
        var prefix = Helpers.GetPrefix(orderByOptions, _options);
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
                    AddStatement($"{clause}, ");
                    continue;
                }
                
                AddStatement($"{clause} ");
                continue;
            }

            if (multipleClauses && !lastItem)
            {
                AddStatement($"{prefix}.{clause}, ");
                continue;
            }
            
            AddStatement($"{prefix}.{clause} ");
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
        var prefix = Helpers.GetPrefix(orderByOptions, _options);
        
        AddStatement($"ORDER BY {prefix}.{clause} ");
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

    public SelectWhereStatement Append(Action<FromStatement> statement)
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

    public SelectWhereStatement AppendIf(Func<bool> condition, string clause)
    {
        if (condition())
        {
            AddStatement($"{clause} ");
        }
        
        return this;
    }
    
    public SelectWhereStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
}