using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public enum Casing
{
    Default,
    UpperCase,
    LowerCase,
    KebabCase,
    PascalCase,
    SnakeCase,
    CamelCase,
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
                        AddStatement(
                            $"{GetPropertyVariant(ConvertCase(property, selectOptions.PropertyCase), selectOptions.Variant)}");
                        AddStatement(", ");
                        continue;
                    }

                    if (appendableAfterFrom.Prefix is not null)
                    {
                        AddStatement(
                            $"{appendableAfterFrom.Prefix}.{GetPropertyVariant(ConvertCase(property, selectOptions.PropertyCase), selectOptions.Variant)}");
                        AddStatement(", ");
                        continue;
                    }

                    AddStatement(
                        $"{table}.{GetPropertyVariant(ConvertCase(property, selectOptions.PropertyCase), selectOptions.Variant)}");
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