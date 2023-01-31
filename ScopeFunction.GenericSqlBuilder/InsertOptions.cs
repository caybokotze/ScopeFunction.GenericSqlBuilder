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
    IInsertOptions WithUpdateOnDuplicateKey();
    IInsertOptions WithInsertIgnore();
    IInsertOptions WithAppendedLastInsertedId();
}

public class InsertOptions : IInsertOptions
{
    public InsertOptions()
    {
        AfterInto = Array.Empty<string>().ToList();
    }
    
    public List<string> AfterInto { get; set; }
    
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

    public IInsertOptions WithInsertIgnore()
    {
        throw new NotImplementedException();
    }

    public IInsertOptions WithAppendedLastInsertedId()
    {
        throw new NotImplementedException();
    }
}