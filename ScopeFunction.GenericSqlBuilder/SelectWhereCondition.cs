namespace ScopeFunction.GenericSqlBuilder;

public interface IWhereCondition
{
    /// <summary>
    /// Will not wrap the clause with any quotations (Just verbatim text)
    /// </summary>
    /// <param name="clause"></param>
    void Equals(string clause);

    /// <summary>
    /// Will wrap the string with 'text'
    /// </summary>
    /// <example>
    /// .Where("Name", e => e.EqualsString("John") => `WHERE Name = 'John'`
    /// </example>
    /// <param name="clause"></param>
    void EqualsString(string clause);

    /// <summary>
    /// Will not add any additional accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    void EqualsNumber(int clause);

    /// <summary>
    /// String literal like e.g. WHERE Name LIKE 'JOHN'
    /// </summary>
    /// <param name="clause"></param>
    void Like(string clause);

    /// <summary>
    /// A verbatim LIKE without accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    void VerbatimLike(string clause);

    /// <summary>
    /// Will append the statement verbatim for cases which are not supported
    /// </summary>
    /// <param name="clause"></param>
    void Append(string clause);
}

public class SelectWhereCondition : Statement, IWhereCondition
{
    private readonly ISelectOptions _options;

    public SelectWhereCondition(Statement statement, ISelectOptions options) : base(statement, options)
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

    /// <summary>
    /// Will not add any additional accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    public void EqualsNumber(int clause)
    {
        AddStatement($"= {clause} ");
    }

    /// <summary>
    /// String literal like e.g. WHERE Name LIKE 'JOHN'
    /// </summary>
    /// <param name="clause"></param>
    public void Like(string clause)
    {
        AddStatement($"LIKE '{clause}' ");
    }
    
    /// <summary>
    /// A verbatim LIKE without accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    public void VerbatimLike(string clause)
    {
        AddStatement($"LIKE {clause} ");
    }

    /// <summary>
    /// Will append the statement verbatim for cases which are not supported
    /// </summary>
    /// <param name="clause"></param>
    public void Append(string clause)
    {
        AddStatement($"{clause} ");
    }
}

public class UpdateWhereCondition : Statement, IWhereCondition
{
    public UpdateWhereCondition(Statement statement, IUpdateOptions options) : base(statement, options)
    {
        
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

    /// <summary>
    /// Will not add any additional accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    public void EqualsNumber(int clause)
    {
        AddStatement($"= {clause} ");
    }

    /// <summary>
    /// String literal like e.g. WHERE Name LIKE 'JOHN'
    /// </summary>
    /// <param name="clause"></param>
    public void Like(string clause)
    {
        AddStatement($"LIKE '{clause}' ");
    }
    
    /// <summary>
    /// A verbatim LIKE without accents or inverted commas
    /// </summary>
    /// <param name="clause"></param>
    public void VerbatimLike(string clause)
    {
        AddStatement($"LIKE {clause} ");
    }

    /// <summary>
    /// Will append the statement verbatim for cases which are not supported
    /// </summary>
    /// <param name="clause"></param>
    public void Append(string clause)
    {
        AddStatement($"{clause} ");
    }
}