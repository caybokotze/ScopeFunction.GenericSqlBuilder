namespace ScopeFunction.GenericSqlBuilder;

public enum Casing
{
    Default,
    PascalCase,
    CamelCase,
    SnakeCase
}

public class SelectStatement : Statement
{
    private readonly ISelectOptions _options;

    public SelectStatement(Statement statement, ISelectOptions options) : base(statement, options)
    {
        _options = options;
    }

    public SelectStatement Append(Action<ISelectStatementBuilder> append)
    {
        var selectStatementBuilder = new SelectStatementBuilder(this, _options);
        append(selectStatementBuilder);
        return this;
    }
    
    public SelectStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }

    public FromStatement From(string table)
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }
        
        if (selectOptions.AppendAfterFrom.Count > 0)
        {
            foreach (var appendableAfterFrom in selectOptions.AppendAfterFrom)
            {
                foreach (var property in appendableAfterFrom.Properties)
                {
                    if (selectOptions.IgnorePrefix)
                    {
                        AddStatement($"{property}");
                        AddStatement(", ");
                        continue;
                    }

                    if (appendableAfterFrom.Prefix is not null)
                    {
                        AddStatement($"{appendableAfterFrom.Prefix}.{property}");
                        AddStatement(", ");
                        continue;
                    }
                
                    AddStatement($"{table}.{property}");
                    AddStatement(", ");
                }
            }
            
            RemoveLast();
            AddStatement(" ");
        }
        
        AddStatement($"FROM {table} ");

        if (selectOptions.Prefix is null)
        {
            _options.WithPropertyPrefix(table);
        }
        
        return new FromStatement(this, _options);
    }
}