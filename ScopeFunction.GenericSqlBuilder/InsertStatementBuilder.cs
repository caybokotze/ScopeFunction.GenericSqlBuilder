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

public interface IInsertStatementBuilder
{
    InsertStatement Insert(string[] properties);
    InsertStatement Insert(string[] properties, Action<IInsertOptions> options);
    InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new();
    InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions> options) where T : class, new();
    InsertStatement Insert<T>() where T : class, new ();
    InsertStatement Insert<T>(Action<IInsertOptions> options) where T : class, new();
}

public class InsertStatementBuilder : Statement, IInsertStatementBuilder
{
    private readonly IInsertOptions _options;
    
    public InsertStatementBuilder(Statement statement, IInsertOptions options) : base(statement, options)
    {
        _options = options;
    }

    public InsertStatementBuilder()
    {
        _options = new InsertOptions();
    }

    public InsertStatement Insert(params string[] properties)
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert(string[] properties, Action<IInsertOptions> options)
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties) where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Func<T, string[]> properties, Action<IInsertOptions> options) where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>() where T : class, new()
    {
        throw new NotImplementedException();
    }

    public InsertStatement Insert<T>(Action<IInsertOptions> options) where T : class, new()
    {
        throw new NotImplementedException();
    }
}

public class InsertStatement : Statement
{
    private readonly Statement _statement;
    private readonly IInsertOptions _options;

    public InsertStatement(Statement statement, IInsertOptions options) : base(statement, options)
    {
        _statement = statement;
        _options = options;
    }

    public Finalise Into(string table)
    {
        return new Finalise(_statement, _options);
    }
}