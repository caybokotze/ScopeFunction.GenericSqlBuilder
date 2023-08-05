using ScopeFunction.GenericSqlBuilder.Common;

namespace ScopeFunction.GenericSqlBuilder;

public interface IOrderByOptions : IOptions
{
    void WithDesc();
    void WithAsc();
}

public class OrderByOptions : Options, IOrderByOptions
{
    public bool IsDesc { get; set; }
    public bool UseSelectPrefix { get; set; }
    public void WithPropertyPrefix(string prefix)
    {
        Prefix = prefix;
    }

    public void WithoutPropertyPrefix()
    {
        IgnorePrefix = true;
    }

    public void WithDesc()
    {
        IsDesc = true;
    }

    public void WithAsc()
    {
        IsDesc = false;
    }
}