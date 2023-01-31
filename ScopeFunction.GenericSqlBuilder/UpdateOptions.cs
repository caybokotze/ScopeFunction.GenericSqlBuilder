using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IUpdateOptions : IOptions
{
    IUpdateOptions WithSqlVariant(Variant variant);
    IUpdateOptions WithPropertyCasing(Casing casing);
    IUpdateOptions WithProperty(string property);
    /// <summary>
    /// Will try to remove the specified property. If it doesn't exist it will not throw.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    IUpdateOptions WithoutProperty(string property);
    IUpdateOptions WithProperties(IEnumerable<string> properties);
    IUpdateOptions WithoutProperties(IEnumerable<string> properties);
}

public class UpdateOptions : Options, IUpdateOptions
{
    public UpdateOptions()
    {
        RemovedProperties = Enumerable.Empty<string>().ToList();
        AddedProperties = Enumerable.Empty<string>().ToList();
    }
    
    public List<string> RemovedProperties { get; private set; }
    public List<string> AddedProperties { get; private set; }
    public Casing PropertyCase { get; private set; }
    public Variant Variant { get; private set; }
    
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

    public void WithPropertyPrefix(string prefix)
    {
        Prefix = prefix;
    }

    public void WithoutPropertyPrefix()
    {
        IgnorePrefix = true;
    }
}