using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IUpdateOptions
{
    IUpdateOptions WithSqlVariant(Variant variant);
    IUpdateOptions WithPropertyCasing(Casing casing);
    IUpdateOptions WithProperty(string property);
    IUpdateOptions WithoutProperty(string property);
    IUpdateOptions WithProperties(IEnumerable<string> properties);
    IUpdateOptions WithoutProperties(IEnumerable<string> properties);
}

public class UpdateOptions : IUpdateOptions
{
    public IUpdateOptions WithSqlVariant(Variant variant)
    {
        throw new NotImplementedException();
    }

    public IUpdateOptions WithPropertyCasing(Casing casing)
    {
        throw new NotImplementedException();
    }

    public IUpdateOptions WithProperty(string property)
    {
        throw new NotImplementedException();
    }

    public IUpdateOptions WithoutProperty(string property)
    {
        throw new NotImplementedException();
    }

    public IUpdateOptions WithProperties(IEnumerable<string> properties)
    {
        throw new NotImplementedException();
    }

    public IUpdateOptions WithoutProperties(IEnumerable<string> properties)
    {
        throw new NotImplementedException();
    }
}