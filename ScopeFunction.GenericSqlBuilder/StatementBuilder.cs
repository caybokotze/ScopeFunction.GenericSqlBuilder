namespace ScopeFunction.GenericSqlBuilder;

public static class StatementBuilder 
{
    public static string Build(IEnumerable<string> statements)
    {
        return statements.Aggregate(string.Empty, (current, statement) => current + statement).TrimEnd(' ');
    }

    public static List<string> GetSelectProperties<T>(SelectOptions options) where T : new()
    {
        var typeProperties = GetSelectProperties<T>();

        foreach (var item in options.WithoutProperties
                     .Where(item => typeProperties
                         .Contains(item, StringComparer.CurrentCultureIgnoreCase)))
        {
            typeProperties.Remove(item);
        }

        typeProperties.AddRange(options.WithProperties);

        return typeProperties;
    }
    
    public static List<string> GetSelectProperties<T>() where T : new ()
    {
        var type = typeof(T);
        return type.GetProperties().Select(property => property.Name).ToList();
    }
}