using ScopeFunction.GenericSqlBuilder.Exceptions;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public class InsertStatement : Statement
{
    private readonly Statement _statement;
    private readonly IInsertOptions _options;

    public InsertStatement(Statement statement, IInsertOptions options) : base(statement)
    {
        _statement = statement;
        _options = options;
    }

    public Finalise Into(string table)
    {
        if (_options is not InsertOptions io)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.InsertOptionCastException);
        }

        if (io.PropertiesToUpdate.Count > 0 && io.PropertiesToNotUpdate.Count > 0)
        {
            throw new InvalidStatementException(SqlBuilderErrorConstants.UpdateAndNotUpdateNotAllowed);
        }
        
        io.AppendAfterIntoStatement.AddRange(io.AddedProperties);
        
        AddStatement($"INTO {table} ");

        if (io.AppendAfterIntoStatement.Count > 0)
        {
            AddStatement("(");
            foreach (var property in io.AppendAfterIntoStatement)
            {
                if (io.RemovedProperties.Contains(property))
                {
                    continue;
                }
                
                AddStatement($"{GetPropertyVariant(ConvertCase(property, io.PropertyCase), io.Variant)}");
                AddStatement(", ");
            }
            
            RemoveLast();
            AddStatement(")");
            AddStatement(" ");
        }

        if (io.AppendAfterIntoStatement.Count > 0)
        {
            AddStatement("VALUES ");
            AddStatement("(");

            foreach (var property in io.AppendAfterIntoStatement)
            {
                if (io.RemovedProperties.Contains(property))
                {
                    continue;
                }
                
                AddStatement($"@{property}");
                AddStatement(", ");
            }
            
            RemoveLast();
            AddStatement(")");
            AddStatement(" ");
        }

        if (io.UpdateOnDuplicateKey)
        {
            if (io.PropertiesToUpdate.Count > 0)
            {
                AddStatement("ON DUPLICATE KEY UPDATE ");

                foreach (var property in io.PropertiesToUpdate)
                {
                    if (io.RemovedProperties.Contains(property))
                    {
                        continue;
                    }
                    
                    AddStatement($"{GetPropertyVariant(ConvertCase(property, io.PropertyCase), io.Variant)} = @{property}");
                    AddStatement(", ");
                }
            }

            if (io.PropertiesToNotUpdate.Count > 0)
            {
                AddStatement("ON DUPLICATE KEY UPDATE ");
                
                foreach (var property in io.AppendAfterIntoStatement)
                {
                    if (io.RemovedProperties.Contains(property))
                    {
                        continue;
                    }
                    
                    if (io.PropertiesToNotUpdate.Contains(property))
                    {
                        continue;
                    }
                    
                    AddStatement($"{GetPropertyVariant(ConvertCase(property, io.PropertyCase), io.Variant)} = @{property}");
                    AddStatement(", ");
                }
            }
        }
        
        RemoveLast();
        
        return new Finalise(_statement, _options);
    }
}