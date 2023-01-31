namespace ScopeFunction.GenericSqlBuilder.Common;

public interface IOptions
{
    /// <summary>
    /// Specify a prefix which overrides the default table name
    /// </summary>
    /// <example>
    /// Instead of the default behaviour SELECT * FROM c WHERE c.Prop1, c.Prop2...
    /// You get SELECT * FROM c WHERE {prefix}.Prop1, {prefix}.Prop2...
    /// </example>
    /// <param name="prefix"></param>
    public void WithPropertyPrefix(string prefix);
    
    /// <summary>
    /// Remove the default prefix set with the table in the SELECT clause
    /// </summary>
    /// <example>
    /// Instead of the default behaviour SELECT * FROM c WHERE c.Prop1, c.Prop2...
    /// You get SELECT * FROM c WHERE Prop1, Prop2...
    /// </example>
    public void WithoutPropertyPrefix();
}

public class Options
{
    public bool IgnorePrefix { get; set; }
    public string? Prefix { get; set; }
}