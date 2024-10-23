using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface ISelectOptions : IOptions
{
    ISelectOptions WithSqlVariant(Variant variant);
    ISelectOptions WithPropertyCasing(Casing casing);
    ISelectOptions WithProperty(string property);
    ISelectOptions WithoutProperty(string property);
    ISelectOptions WithProperties(IEnumerable<string> properties);
    ISelectOptions WithoutProperties(IEnumerable<string> properties);
    ISelectOptions WithTop(int value);
    ISelectOptions WithSplitOn(string splitOn);
}

public class SelectOptions : Options, ISelectOptions
{
    public SelectOptions()
    {
        AddedProperties = new List<string>();
        RemovedProperties = new List<string>();
        AppendAfterFromStatement = new List<AppendableAfterFrom>();
        var configuration = GenericQueryBuilderSettings.GenericSqlBuilderConfiguration;
        Variant = configuration.Variant;
        PropertyCase = configuration.Casing;
    }
    
    public List<string> AddedProperties { get; }
    public List<string> RemovedProperties { get; }
    public List<AppendableAfterFrom> AppendAfterFromStatement { get; set; }
    public Casing PropertyCase { get; private set; }
        
    public Variant Variant { get; private set; }
        
    public int? TopValue { get; private set; }
        
    public bool IsAppendWhere { get; set; }
    public bool IsAppendSelect { get; set; }
    
    public string? SplitOn { get; set; }

    public ISelectOptions WithPropertyCasing(Casing casing)
    {
        PropertyCase = casing;
        return this;
    }
        
    public ISelectOptions WithSqlVariant(Variant variant)
    {
        Variant = variant;
        return this;
    }

    public ISelectOptions WithProperty(string property)
    {
        AddedProperties.Add(property);
        return this;
    }

    public ISelectOptions WithoutProperty(string property)
    {
        RemovedProperties.Add(property);
        return this;
    }

    public ISelectOptions WithProperties(IEnumerable<string> properties)
    {
        AddedProperties.AddRange(properties);
        return this;
    }

    public ISelectOptions WithoutProperties(IEnumerable<string> properties)
    {
        RemovedProperties.AddRange(properties);
        return this;
    }

    public ISelectOptions WithTop(int value)
    {
        TopValue = value;
        return this;
    }

    public ISelectOptions WithSplitOn(string splitOn)
    {
        SplitOn = splitOn;
        return this;
    }

    /// <summary>
    /// Specifies the prefix which will be appended to the SELECT or WHERE clause.
    /// This method is void and should be placed at the end of the builder
    /// </summary>
    /// <param name="prefix"></param>
    public void WithPropertyPrefix(string prefix)
    {
        Prefix = prefix;
    }

    /// <summary>
    /// Specifies the prefix which will be removed from the SELECT or WHERE clause.
    /// This method is void and should be placed at the end of the builder
    /// </summary>
    public void WithoutPropertyPrefix()
    {
        IgnorePrefix = true;
    }
}