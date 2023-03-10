namespace ScopeFunction.GenericSqlBuilder;

public static class StatementBuilder 
{
    public static string Build(IEnumerable<string> statements)
    {
        return statements.Aggregate(string.Empty, (current, statement) => current + statement).TrimEnd(' ');
    }

    public static List<string> GetSelectProperties<T>(SelectOptions options) where T : class, new()
    {
        var typeProperties = GetSelectProperties<T>();

        foreach (var item in options.RemovedProperties
                     .Where(item => typeProperties
                         .Contains(item, StringComparer.CurrentCultureIgnoreCase)))
        {
            typeProperties.Remove(item);
        }

        typeProperties.AddRange(options.AddedProperties);

        return typeProperties;
    }
    
    public static List<string> GetSelectProperties<T>() where T : class, new ()
    {
        var type = typeof(T);
        return type.GetProperties().Select(property => property.Name).ToList();
    }
}