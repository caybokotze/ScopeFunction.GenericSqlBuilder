using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.Helpers;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public class SelectStatement : Statement
{
    private readonly ISelectOptions _options;

    public SelectStatement(Statement statement, ISelectOptions options) : base(statement)
    {
        _options = options;
    }

    public SelectStatement AppendSelect(Action<ISelectStatementBuilder> append)
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }

        selectOptions.IsAppendSelect = true;
        
        var selectStatementBuilder = new SelectStatementBuilder(this, selectOptions);
        append(selectStatementBuilder);
        return this;
    }
    
    public SelectStatement Append(string clause, bool withTrailingComma = false)
    {
        if (withTrailingComma)
        {
            AddStatement($"{clause}, ");
        }

        if (!withTrailingComma)
        {
            AddStatement($"{clause} ");
        }

        return this;
    }

    public FromStatement From(string table)
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }
        
        if (selectOptions.AppendAfterFromStatement.Count > 0)
        {
            foreach (var appendableAfterFrom in selectOptions.AppendAfterFromStatement)
            {
                if (selectOptions.SplitOn is not null)
                {
                    AddStatement(
                        $"{GetPrefix(selectOptions, table, appendableAfterFrom.Prefix)}{GetPropertyVariant(ConvertCase(selectOptions.SplitOn, selectOptions.PropertyCase), selectOptions.Variant)}");
                    AddStatement(", ");
                }
                
                foreach (var property in appendableAfterFrom.Properties)
                {
                    if (selectOptions.SplitOn is not null)
                    {
                        if (property.Equals(selectOptions.SplitOn, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }    
                    }
                    
                    AddStatement($"{GetPrefix(selectOptions, table, appendableAfterFrom.Prefix)}{GetPropertyVariant(ConvertCase(property, selectOptions.PropertyCase), selectOptions.Variant)}");
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