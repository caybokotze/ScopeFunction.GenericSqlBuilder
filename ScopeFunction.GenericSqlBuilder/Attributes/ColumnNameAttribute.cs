namespace ScopeFunction.GenericSqlBuilder.Attributes;

/// <summary>
/// Specifies the database column name for a property.
/// When applied, this name will be used instead of the auto-converted property name.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ColumnNameAttribute : Attribute
{
    public string Name { get; }

    public ColumnNameAttribute(string name)
    {
        Name = name;
    }
}
