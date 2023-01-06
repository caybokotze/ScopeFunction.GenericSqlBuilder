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

        if (_options is not SelectOptions so)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        if (so.IsAppendSelect)
        {
            Console.WriteLine("Hey there");
        }
    }

    public SelectStatement Append(Action<SqlBuilder> append)
    {
        if (_options is not SelectOptions options)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }

        options.IsAppendSelect = true;

        var sqlBuilder = new SqlBuilder(this, options);
        append(sqlBuilder);
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
            foreach (var property in selectOptions.AppendAfterFrom)
            {
                if (selectOptions.IgnorePrefix)
                {
                    AddStatement($"{property}");
                    AddStatement(", ");
                    continue;
                }

                if (selectOptions.Prefix is not null)
                {
                    AddStatement($"{selectOptions.Prefix}.{property}");
                    AddStatement(", ");
                    continue;
                }
                
                AddStatement($"{table}.{property}");
                AddStatement(", ");
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