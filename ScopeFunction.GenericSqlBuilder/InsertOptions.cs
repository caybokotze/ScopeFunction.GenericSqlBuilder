using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IInsertOptions : IOptions
{
    IInsertOptions WithSqlVariant(Variant variant);
    IInsertOptions WithPropertyCasing(Casing casing);
    IInsertOptions WithProperty(string property);
    IInsertOptions WithoutProperty(string property);
    IInsertOptions WithProperties(IEnumerable<string> properties);
    IInsertOptions WithoutProperties(IEnumerable<string> properties);
    
    /// <summary>
    /// This overload will add a 'SET' for all available properties.
    /// </summary>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKey();
    
    /// <summary>
    /// This overload will only add a 'SET' for the provided properties.
    /// </summary>
    /// <param name="onlySet"></param>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKey(params string[] onlySet);
    IInsertOptions WithInsertIgnore();
    IInsertOptions WithAppendedLastInsertedId();
}

public class InsertOptions : IInsertOptions
{
    public InsertOptions()
    {
        AfterInto = new List<string>();
        OnlySet = new List<string>();
    }
    
    public List<string> AfterInto { get; set; }
    public List<string> OnlySet { get; private set; }
    
    public void WithPropertyPrefix(string prefix)
    {
        throw new NotImplementedException();
    }

    public void WithoutPropertyPrefix()
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithSqlVariant(Variant variant)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithPropertyCasing(Casing casing)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithProperty(string property)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithoutProperty(string property)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithProperties(IEnumerable<string> properties)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithoutProperties(IEnumerable<string> properties)
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithUpdateOnDuplicateKey()
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithUpdateOnDuplicateKey(params string[] onlySet)
    {
        OnlySet.AddRange(onlySet);
        return this;
    }

    public IInsertOptions WithInsertIgnore()
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithAppendedLastInsertedId()
    {
        throw new NotImplementedException();
    }
}