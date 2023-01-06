using ScopeFunction.GenericSqlBuilder.Common;

namespace ScopeFunction.GenericSqlBuilder;

public interface IOrderByOptions : IOptions
{
    void WithSelectPrefix();
}

public class OrderByOptions : Options, IOrderByOptions
{
    public bool UseSelectPrefix { get; set; }
    public void WithPropertyPrefix(string prefix)
    {
        Prefix = prefix;
    }

    public void WithoutPropertyPrefix()
    {
        IgnorePrefix = true;
    }

    public void WithSelectPrefix()
    {
        throw new NotImplementedException();
    }
}