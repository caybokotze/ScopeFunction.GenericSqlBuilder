using ScopeFunction.GenericSqlBuilder.Common;

namespace ScopeFunction.GenericSqlBuilder;

public interface IWhereOptions : IOptions
{
    /// <summary>
    /// Groups the where clause body with parens ( ) effectively nesting the statement in a query group
    /// </summary>
    /// <example>
    /// WHERE (id = '1' AND id = '2')
    /// </example>
    public void WithOuterGroup();
    
    /// <summary>
    /// Separate where clause items with OR
    /// </summary>
    public void WithOrSeparator();
    
    /// <summary>
    /// Separate where clause items with AND
    /// </summary>
    public void WithAndSeparator();

    public void WithoutSeparator();
}

public class WhereOptions : Options, IWhereOptions
{
    public bool HasOuterGroup { get; set; }
    public bool HasOrSeparator { get; set; }
    public bool HasAndSeparator { get; set; }
    
    public bool HasNoSeparator { get; set; }

    public void WithOuterGroup()
    {
        HasOuterGroup = true;
    }

    public void WithOrSeparator()
    {
        HasOrSeparator = true;
    }

    public void WithAndSeparator()
    {
        HasAndSeparator = true;
    }

    public void WithoutSeparator()
    {
        HasAndSeparator = false;
        HasOrSeparator = false;
        HasNoSeparator = true;
    }

    public void WithPropertyPrefix(string prefix)
    {
        Prefix = prefix;
    }

    public void WithoutPropertyPrefix()
    {
        IgnorePrefix = true;
    }
}