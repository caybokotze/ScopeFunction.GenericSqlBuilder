using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using ScopeFunction.GenericSqlBuilder.Attributes;
using ScopeFunction.GenericSqlBuilder.Enums;
using static ScopeFunction.GenericSqlBuilder.Common.CaseConverter;

namespace ScopeFunction.GenericSqlBuilder;

/// <summary>
/// Cached metadata for a property, including its name and optional column name override.
/// </summary>
internal sealed class PropertyMetadata
{
    public string Name { get; }
    public string? ColumnNameOverride { get; }

    public PropertyMetadata(string name, string? columnNameOverride)
    {
        Name = name;
        ColumnNameOverride = columnNameOverride;
    }
}

internal static class StatementBuilder
{
    // Cache for property metadata per type - avoids repeated reflection calls
    private static readonly ConcurrentDictionary<Type, PropertyMetadata[]> PropertyCache = new();

    // Cache for column name conversions - avoids repeated case conversion for the same property/casing combination
    private static readonly ConcurrentDictionary<(Type, string, Casing), string> ColumnNameCache = new();

    public static string Build(IEnumerable<string> statements)
    {
        var sb = new StringBuilder();
        foreach (var statement in statements)
        {
            sb.Append(statement);
        }

        // Trim trailing space
        while (sb.Length > 0 && sb[^1] == ' ')
        {
            sb.Length--;
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets cached property metadata for a type. This is called once per type and cached.
    /// </summary>
    private static PropertyMetadata[] GetCachedPropertyMetadata(Type type)
    {
        return PropertyCache.GetOrAdd(type, t =>
            t.GetProperties()
                .Where(property => property.CanRead && property.CanWrite && !property.IsDefined(typeof(IgnorePropertyAttribute), false))
                .Select(property => new PropertyMetadata(
                    property.Name,
                    property.GetCustomAttribute<ColumnNameAttribute>()?.Name))
                .ToArray());
    }

    public static List<string> GetSelectProperties<T>(SelectOptions options) where T : class, new()
    {
        var typeProperties = GetPropertyNames<T>();

        if (options.RemovedProperties.Count > 0)
        {
            var removedSet = new HashSet<string>(options.RemovedProperties);
            typeProperties.RemoveAll(item => removedSet.Contains(item));
        }

        typeProperties.AddRange(options.AddedProperties);

        options.AddedProperties.Clear();
        options.RemovedProperties.Clear();

        return typeProperties;
    }

    public static List<string> GetPropertyNames<T>() where T : new()
    {
        var metadata = GetCachedPropertyMetadata(typeof(T));
        var result = new List<string>(metadata.Length);
        foreach (var prop in metadata)
        {
            result.Add(prop.Name);
        }
        return result;
    }

    public static List<string> GetUpdateProperties<T>(UpdateOptions options) where T : new()
    {
        var typeProperties = GetPropertyNames<T>();

        if (options.RemovedProperties.Count > 0)
        {
            var removedSet = new HashSet<string>(options.RemovedProperties);
            typeProperties.RemoveAll(item => removedSet.Contains(item));
        }

        typeProperties.AddRange(options.AddedProperties);

        return typeProperties;
    }

    /// <summary>
    /// Gets the column name for a property, checking for ColumnNameAttribute first,
    /// then falling back to case conversion. Results are cached.
    /// </summary>
    public static string GetColumnName(Type? type, string propertyName, Casing casing)
    {
        if (type is null)
        {
            return ConvertCase(propertyName, casing);
        }

        return ColumnNameCache.GetOrAdd((type, propertyName, casing), key =>
        {
            var (t, propName, c) = key;
            var metadata = GetCachedPropertyMetadata(t);

            foreach (var prop in metadata)
            {
                if (prop.Name == propName)
                {
                    return prop.ColumnNameOverride ?? ConvertCase(propName, c);
                }
            }

            // Property not found in metadata (might be dynamically added), fall back to case conversion
            return ConvertCase(propName, c);
        });
    }

    /// <summary>
    /// Gets the column name for a property, checking for ColumnNameAttribute first,
    /// then falling back to case conversion. Results are cached.
    /// </summary>
    public static string GetColumnName<T>(string propertyName, Casing casing) where T : new()
    {
        return GetColumnName(typeof(T), propertyName, casing);
    }
}