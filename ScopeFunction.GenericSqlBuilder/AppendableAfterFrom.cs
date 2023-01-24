namespace ScopeFunction.GenericSqlBuilder;

public class AppendableAfterFrom
{
    public AppendableAfterFrom(string[] properties)
    {
        Properties = properties;
    }
    
    public AppendableAfterFrom(string[] properties, string? prefix)
    {
        Properties = properties;
        Prefix = prefix;
    }
    
    public string[] Properties { get; }
    public string? Prefix { get; }
}