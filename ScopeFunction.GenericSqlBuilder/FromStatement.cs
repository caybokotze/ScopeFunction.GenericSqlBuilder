using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Exceptions;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;
using static ScopeFunction.GenericSqlBuilder.Common.VariantHelpers;

namespace ScopeFunction.GenericSqlBuilder;

public class FromStatement : Statement, IBuildable
{
    private readonly ISelectOptions _options;
    private readonly WhereOptions? _whereOptions;

    public FromStatement(
        Statement statement,
        ISelectOptions options) : base(statement)
    {
        _options = options;
    }
    
    public FromStatement(Statement statement, ISelectOptions options, WhereOptions whereOptions) : base(statement)
    {
        _options = options;
        _whereOptions = whereOptions;
    }
    
    public string Build()
    {
        SetSelectOptions(_options);
        return BuildStatement();
    }

    private static string GetPrefixAndClause(string clause, string prefix)
    {
        return $"{prefix}.{clause}";
    }

    private static string GetSeparationClause(WhereOptions whereOptions)
    {
        return whereOptions.HasAndSeparator switch
        {
            true when whereOptions.HasOrSeparator => throw new InvalidStatementException(
                Errors.OrAndAndNotAllowed),
            true => "AND",
            false when whereOptions.HasOrSeparator => "OR",
            _ => "OR"
        };
    }

    private void AddWhereClauses(IEnumerable<string> clauses, WhereOptions whereOptions)
    {
        var prefix = Helpers.GetPrefix(whereOptions, _options);
        var separator = GetSeparationClause(whereOptions);

        if (!string.IsNullOrEmpty(prefix))
        {
            foreach (var clause in clauses)
            {
                AddStatement($"{GetPrefixAndClause(clause, prefix)} ");
                AddStatement($"{separator} ");
            }

            RemoveLast();
            return;
        }

        foreach (var clause in clauses)
        {
            AddStatement($"{clause} ");
            AddStatement($"{separator} ");
        }

        RemoveLast();
    }
    
    private void AddWhereOrSeparator(WhereOptions whereOptions)
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        switch (selectOptions.IsAppendWhere)
        {
            case false:
                AddStatement("WHERE ");
                break;
            case true when _whereOptions is null:
                AddStatement($"{GetSeparationClause(whereOptions)} ");
                break;
        }
    }

    public FromStatement Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }

    # region JOINING
    public JoinStatement Join(string clause)
    {
        AddStatement($"JOIN {clause} ");
        return new JoinStatement(this, _options);
    }
    
    public JoinStatement LeftJoin(string clause)
    {
        AddStatement($"LEFT JOIN {clause} ");
        return new JoinStatement(this, _options);
    }

    public JoinStatement RightJoin(string clause)
    {
        AddStatement($"RIGHT JOIN {clause} ");
        return new JoinStatement(this, _options);
    }

    public JoinStatement InnerJoin(string clause)
    {
        AddStatement($"INNER JOIN {clause} ");
        return new JoinStatement(this, _options);
    }
    #endregion

    #region With Options
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clauses"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public SelectWhereStatement Where(IEnumerable<string> clauses, Action<IWhereOptions> options)
    {
        var whereOptions = new WhereOptions();
        options(whereOptions);
        
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }

        if (!whereOptions.HasOuterGroup)
        {
            AddWhereOrSeparator(whereOptions);
            AddWhereClauses(clauses, whereOptions);
            return new SelectWhereStatement(this, _options);
        }

        switch (selectOptions.IsAppendWhere)
        {
            case false:
                AddStatement("WHERE ");
                break;
            case true when whereOptions.HasOuterGroup is false:
                AddStatement($"{GetSeparationClause(whereOptions)} ");
                break;
        }

        AddStatement("(");
        AddWhereClauses(clauses, whereOptions);
        TrimLast();
        AddStatement(") ");

        return new SelectWhereStatement(this, _options);
    }

    public SelectWhereStatement NestedWhere(Action<FromStatement> nestedWhere)
    {
        var fromStatement = new FromStatement(this, _options);
        AddStatement("( ");
        nestedWhere(fromStatement);
        AddStatement(") ");
        return new SelectWhereStatement(this, _options);
    }
    
    /// <summary>
    /// The Generic Where overload allows you to have strongly defined type names when defining your property names in list format.
    /// The options are required in this iteration
    /// </summary>
    /// <example>
    /// SelectAll().From("c").Where(w => new [] {$"{nameof(w.FirstName)} = 'John'", o => o.WithoutPrefix())
    /// </example>
    /// <param name="clause"></param>
    /// <param name="options"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public SelectWhereStatement Where<T>(Func<T, string[]> clause, Action<IWhereOptions> options) where T : new()
    {
        var clauses = clause(new T());
        var whereOptions = new WhereOptions();
        options(whereOptions);
        
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }

        if (!whereOptions.HasOuterGroup)
        {
            AddWhereOrSeparator(whereOptions);
            AddWhereClauses(clauses, whereOptions);
            return new SelectWhereStatement(this, _options);
        }

        switch (selectOptions.IsAppendWhere)
        {
            case false:
                AddStatement("WHERE ");
                break;
            case true when whereOptions.HasOuterGroup is false:
                AddStatement($"{GetSeparationClause(whereOptions)} ");
                break;
        }

        AddStatement("(");
        AddWhereClauses(clauses, whereOptions);
        TrimLast();
        AddStatement(") ");
        return new SelectWhereStatement(this, _options);
    }
    #endregion

    #region Without Options
    
    /// <summary>
    /// The Generic Where overload allows you to have strongly defined type names when defining your property names in list format.
    /// The options are not required for this overload
    /// </summary>
    /// <example>
    /// SelectAll().From("c").Where(w => new [] {$"{nameof(w.FirstName)} = 'John'")
    /// </example>
    /// <param name="clause"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public SelectWhereStatement Where<T>(Func<T, string[]> clause) where T : new()
    {
        var clauses = clause(new T());
        var whereOptions = new WhereOptions();
        AddWhereOrSeparator(whereOptions);
        AddWhereClauses(clauses, whereOptions);
        return new SelectWhereStatement(this, _options);
    }
    
    /// <summary>
    /// The explicit where will not inherit any options set on SELECT. It will just add the statement added verbatim.
    /// </summary>
    /// <param name="clause"></param>
    /// <param name="applyPrefix"></param>
    /// <returns></returns>
    public SelectWhereStatement Where(string clause, bool applyPrefix = false)
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        var prefix = Helpers.GetPrefix(new WhereOptions(), _options);

        switch (selectOptions.IsAppendWhere)
        {
            case false:
                AddStatement(applyPrefix && !string.IsNullOrEmpty(prefix) 
                    ? $"WHERE {prefix}.{clause}" 
                    : $"WHERE {clause} ");
                break;
            case true:
                AddStatement(applyPrefix && !string.IsNullOrEmpty(prefix) 
                    ? $"AND {prefix}.{clause}" 
                    : $"AND {clause} ");
                break;
        }

        return new SelectWhereStatement(this, _options);
    }

    public SelectWhereStatement Where(string clause, Action<IWhereOptions> options)
    {
        if (_options is not SelectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        var whereOptions = new WhereOptions();
        
        options(whereOptions);
        
        AddWhereOrSeparator(whereOptions);
        
        AddStatement($"{clause} ");

        return new SelectWhereStatement(this, _options);
    }

    public SelectWhereStatement Where<T>(Func<T, string> clause, Action<SelectWhereCondition> condition) where T : new()
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        var whereOptions = new WhereOptions();
        var prefix = Helpers.GetPrefix(whereOptions, _options);
        var clauseSegment = clause(new T());
        
        AddWhereOrSeparator(whereOptions);
        
        if (!string.IsNullOrEmpty(prefix))
        {
            AddStatement($"{prefix}.{GetPropertyVariant(ConvertCase(clauseSegment, selectOptions.PropertyCase), selectOptions.Variant)} ");
        }

        if (string.IsNullOrEmpty(prefix))
        {
            AddStatement($"{GetPropertyVariant(ConvertCase(clauseSegment, selectOptions.PropertyCase), selectOptions.Variant)} ");
        }
        
        var whereCondition = new SelectWhereCondition(this, _options);
        condition(whereCondition);
        return new SelectWhereStatement(this, _options);
    }
    
    public SelectWhereStatement Where<T>(Func<T, string> clause, Action<SelectWhereCondition> condition, Action<IWhereOptions> options) where T : new()
    {
        if (_options is not SelectOptions selectOptions)
        {
            throw new InvalidCastException(
                Errors.SelectOptionCastException);
        }
        
        var whereOptions = new WhereOptions();
        options(whereOptions);
        
        var prefix = Helpers.GetPrefix(whereOptions, _options);
        var clauseSegment = clause(new T());
        
        AddWhereOrSeparator(whereOptions);
        
        if (!string.IsNullOrEmpty(prefix))
        {
            AddStatement($"{prefix}.{GetPropertyVariant(ConvertCase(clauseSegment, selectOptions.PropertyCase), selectOptions.Variant)} ");
        }

        if (string.IsNullOrEmpty(prefix))
        {
            AddStatement($"{GetPropertyVariant(ConvertCase(clauseSegment, selectOptions.PropertyCase), selectOptions.Variant)} ");
        }
        
        var whereCondition = new SelectWhereCondition(this, _options);
        condition(whereCondition);
        return new SelectWhereStatement(this, _options);
    }
    
    /// <summary>
    /// The array overload does inherit options set on SELECT. Meaning the table prefix will automatically be added unless reverted via WHERE clause options.
    /// </summary>
    /// <param name="clauses"></param>
    /// <returns></returns>
    public SelectWhereStatement Where(IEnumerable<string> clauses)
    {
        var whereOptions = new WhereOptions();
        AddWhereOrSeparator(whereOptions);
        AddWhereClauses(clauses, whereOptions);
        return new SelectWhereStatement(this, _options);
    }
    
    #endregion
}