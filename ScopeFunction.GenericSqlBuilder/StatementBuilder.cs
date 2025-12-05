using System.Reflection;
using ScopeFunction.GenericSqlBuilder.Attributes;
using ScopeFunction.GenericSqlBuilder.Enums;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;

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

        options.AddedProperties.Clear();
        options.RemovedProperties.Clear();

        return typeProperties;
    }

    public static List<string> GetPropertyNames<T>() where T : new ()
    {
        var type = typeof(T);
        return type.GetProperties()
            .Where(property => property.CanRead && property.CanWrite && !property.IsDefined(typeof(IgnorePropertyAttribute), false))
            .Select(property => property.Name)
            .ToList();
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

    /// <summary>
    /// Gets the column name for a property, checking for ColumnNameAttribute first,
    /// then falling back to case conversion.
    /// </summary>
    public static string GetColumnName(Type type, string propertyName, Casing casing)
    {
        var property = type.GetProperty(propertyName);

        if (property is null)
        {
            return ConvertCase(propertyName, casing);
        }

        var columnNameAttribute = property.GetCustomAttribute<ColumnNameAttribute>();

        if (columnNameAttribute is not null)
        {
            return columnNameAttribute.Name;
        }

        return ConvertCase(propertyName, casing);
    }

    /// <summary>
    /// Gets the column name for a property, checking for ColumnNameAttribute first,
    /// then falling back to case conversion.
    /// </summary>
    public static string GetColumnName<T>(string propertyName, Casing casing) where T : new()
    {
        return GetColumnName(typeof(T), propertyName, casing);
    }
}