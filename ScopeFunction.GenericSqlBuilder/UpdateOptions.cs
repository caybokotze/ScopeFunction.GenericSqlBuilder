using System.Linq.Expressions;
using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IUpdateOptions
{
    IUpdateOptions WithSqlVariant(Variant variant);
    IUpdateOptions WithPropertyCasing(Casing casing);
}

public interface IUpdateOptions<T> : IUpdateOptions
{
    IUpdateOptions WithProperty(string property);
    /// <summary>
    /// Will try to remove the specified property. If it doesn't exist it will not throw.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    IUpdateOptions WithoutProperty(string property);
    IUpdateOptions WithProperties(IEnumerable<string> properties);
    IUpdateOptions WithoutProperties(IEnumerable<string> properties);
    IUpdateOptions<T> WithoutProperties(params Expression<Func<T, object?>>[] properties);
}

public class UpdateOptions<T> : UpdateOptions, IUpdateOptions<T>
{
    public IUpdateOptions<T> WithoutProperties(params Expression<Func<T, object?>>[] properties)
    {
        foreach (var segment in properties)
        {
            if (segment.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;
                RemovedProperties.Add(name);
            }

            if (segment.Body is UnaryExpression unaryExpression)
            {
                var name = (unaryExpression.Operand as MemberExpression)?.Member.Name ?? string.Empty;
                RemovedProperties.Add(name);
            }
        }

        return this;
    }
     
    public IUpdateOptions WithProperty(string property)
    {
        AddedProperties.Add(property);
        return this;
    }

    public IUpdateOptions WithoutProperty(string property)
    {
        RemovedProperties.Add(property);
        return this;
    }

    public IUpdateOptions WithProperties(IEnumerable<string> properties)
    {
        AddedProperties.AddRange(properties);
        return this;
    }

    public IUpdateOptions WithoutProperties(IEnumerable<string> properties)
    {
        RemovedProperties.AddRange(properties);
        return this;
    }
}

public class UpdateOptions : IUpdateOptions
{
    public UpdateOptions()
    {
        RemovedProperties = Enumerable.Empty<string>().ToList();
        AddedProperties = Enumerable.Empty<string>().ToList();
        var configuration = GenericQueryBuilderSettings.GenericSqlBuilderConfiguration;
        Variant = configuration.Variant;
        PropertyCase = configuration.Casing;
        Table = string.Empty;
    }
    
    public List<string> RemovedProperties { get; private set; }
    public List<string> AddedProperties { get; private set; }
    public Casing PropertyCase { get; private set; }
    public Variant Variant { get; private set; }
    public string Table { get; set; }
    
    
    public IUpdateOptions WithSqlVariant(Variant variant)
    {
        Variant = variant;
        return this;
    }

    public IUpdateOptions WithPropertyCasing(Casing casing)
    {
        PropertyCase = casing;
        return this;
    }

}