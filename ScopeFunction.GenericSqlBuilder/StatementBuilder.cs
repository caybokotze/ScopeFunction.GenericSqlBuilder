namespace ScopeFunction.GenericSqlBuilder;

internal static class StatementBuilder 
{
    public static string Build(IEnumerable<string> statements)
    {
        return statements.Aggregate(string.Empty, (current, statement) => current + statement).TrimEnd(' ');
    }

    public static List<string> GetSelectProperties<T>(SelectOptions options) where T : class, new()
    {
        var typeProperties = GetPropertyNames<T>();

        foreach (var item in options.RemovedProperties
                     .Where(item => typeProperties
                         .Contains(item)))
        {
            typeProperties.Remove(item);
        }

        typeProperties.AddRange(options.AddedProperties);

        return typeProperties;
    }

    public static List<string> GetPropertyNames<T>() where T : new ()
    {
        var type = typeof(T);
        return type.GetProperties().Select(property => property.Name).ToList();
    }

    public static List<string> GetUpdateProperties<T>(UpdateOptions options) where T : new()
    {
        var typeProperties = GetPropertyNames<T>();

        foreach (var item in options.RemovedProperties
                     .Where(item => typeProperties
                         .Contains(item)))
        {
            typeProperties.Remove(item);
        }

        typeProperties.AddRange(options.AddedProperties);

        return typeProperties;
    }
}