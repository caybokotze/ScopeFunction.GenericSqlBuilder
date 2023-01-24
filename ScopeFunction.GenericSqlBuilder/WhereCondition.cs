namespace ScopeFunction.GenericSqlBuilder;

public class WhereCondition : Statement
{
    private readonly ISelectOptions _options;

    public WhereCondition(Statement statement, ISelectOptions options) : base(statement, options)
    {
        _options = options;
    }

    /// <summary>
    /// Will not wrap the clause with any quotations (Just verbatim text)
    /// </summary>
    /// <param name="clause"></param>
    public void Equals(string clause)
    {
        AddStatement($"= {clause} ");
    }

    /// <summary>
    /// Will wrap the string with 'text'
    /// </summary>
    /// <example>
    /// .Where("Name", e => e.EqualsString("John") => `WHERE Name = 'John'`
    /// </example>
    /// <param name="clause"></param>
    public void EqualsString(string clause)
    {
        AddStatement($"= '{clause}' ");
    }

    public void EqualsNumber(int clause)
    {
        AddStatement($"= {clause} ");
    }

    public void Like(string clause)
    {
        AddStatement($"LIKE '{clause}' ");
    }
    
    public void VerbatimLike(string clause)
    {
        AddStatement($"LIKE {clause} ");
    }
}