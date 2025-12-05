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

    public AppendableAfterFrom(string[] properties, string? prefix, Type? sourceType)
    {
        Properties = properties;
        Prefix = prefix;
        SourceType = sourceType;
    }

    public string[] Properties { get; }
    public string? Prefix { get; }
    public Type? SourceType { get; }
}