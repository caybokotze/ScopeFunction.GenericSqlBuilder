using System.Linq.Expressions;
using ScopeFunction.GenericSqlBuilder.Common;
using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public interface IInsertOptions
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
    /// This overload allows for the properties to be 'SET' which should be added to the update statement.
    /// If include is set to true, only the properties provided will be included in the update statement.
    /// If include is set to false, every property except the properties provided will be included in the update statement. 
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKey(params string[] properties);
    
    /// <summary>
    /// This overload exists to remove properties from the update 'SET' statement on insert
    /// Only the values provided will be ignored when compared to the values provided in the insert statement.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKeyIgnore(params string[] properties);
    IInsertOptions WithInsertIgnore();
    IInsertOptions WithAppendedLastInsertedId();
}

public interface IInsertOptions<T> : IInsertOptions
{
    /// <summary>
    /// This overload allows for the properties to be 'SET' which should be added to the update statement.
    /// If properties are provided it will override all properties built up by the generic builder. (Use with caution)
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKey(params Expression<Func<T, object?>>[] properties);
    
    /// <summary>
    /// This overload exists to remove properties from the update 'SET' statement on insert
    /// The update statement will be built with the generic builder and only the values provided will be ignored.
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    IInsertOptions WithUpdateOnDuplicateKeyIgnore(params Expression<Func<T, object?>>[] properties);
}

public class InsertOptions<T> : InsertOptions, IInsertOptions<T>
{
    public IInsertOptions WithUpdateOnDuplicateKey(params Expression<Func<T, object?>>[] properties)
    {
        foreach (var segment in properties)
        {
            if (segment.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;
                UpdateOnDuplicateKey = true;
                PropertiesToUpdate.Add(name);
                continue;
            }

            if (segment.Body is UnaryExpression unaryExpression)
            {
                var name = (unaryExpression.Operand as MemberExpression)?.Member.Name ?? string.Empty;
                UpdateOnDuplicateKey = true;
                PropertiesToUpdate.Add(name);
            }
        }

        return this;
    }

    public IInsertOptions WithUpdateOnDuplicateKeyIgnore(params Expression<Func<T, object?>>[] properties)
    {
        foreach (var segment in properties)
        {
            if (segment.Body is MemberExpression memberExpression)
            {
                var name = memberExpression.Member.Name;
                UpdateOnDuplicateKey = true;
                PropertiesToNotUpdate.Add(name);
                continue;
            }

            if (segment.Body is UnaryExpression unaryExpression)
            {
                var name = (unaryExpression.Operand as MemberExpression)?.Member.Name ?? string.Empty;
                UpdateOnDuplicateKey = true;
                PropertiesToNotUpdate.Add(name);
            }
        }

        return this;
    }
}

public class InsertOptions : IInsertOptions
{
    public InsertOptions()
    {
        AppendAfterIntoStatement = new List<string>();
        PropertiesToUpdate = new List<string>();
        PropertiesToNotUpdate = new List<string>();
        AddedProperties = new List<string>();
        RemovedProperties = new List<string>();
    }
    
    public List<string> AppendAfterIntoStatement { get; }
    public List<string> PropertiesToUpdate { get; }
    public List<string> PropertiesToNotUpdate { get; }
    public List<string> AddedProperties { get; }
    public List<string> RemovedProperties { get; }
    public bool InsertIgnore { get; private set; }
    public bool AppendLastInsertedId { get; private set; }
    public bool UpdateOnDuplicateKey { get; protected set; }
    public Casing PropertyCase { get; private set; }
    public Variant Variant { get; private set; }

    public IInsertOptions WithSqlVariant(Variant variant)
    {
        Variant = variant;
        return this;
    }

    public IInsertOptions WithPropertyCasing(Casing casing)
    {
        PropertyCase = casing;
        return this;
    }

    public IInsertOptions WithProperty(string property)
    {
        AddedProperties.Add(property);
        return this;
    }

    public IInsertOptions WithoutProperty(string property)
    {
        RemovedProperties.Add(property);
        return this;
    }

    public IInsertOptions WithProperties(IEnumerable<string> properties)
    {
        AddedProperties.AddRange(properties);
        return this;
    }

    public IInsertOptions WithoutProperties(IEnumerable<string> properties)
    {
        RemovedProperties.AddRange(properties);
        return this;
    }

    public IInsertOptions WithUpdateOnDuplicateKey()
    {
        UpdateOnDuplicateKey = true;
        return this;
    }

    public IInsertOptions WithUpdateOnDuplicateKey(params string[] properties)
    {
        PropertiesToUpdate.AddRange(properties);
        UpdateOnDuplicateKey = true;
        return this;
    }

    public IInsertOptions WithUpdateOnDuplicateKeyIgnore(params string[] properties)
    {
        PropertiesToNotUpdate.AddRange(properties);
        UpdateOnDuplicateKey = true;
        return this;
    }

    public IInsertOptions WithInsertIgnore()
    {
        InsertIgnore = true;
        return this;
    }

    public IInsertOptions WithAppendedLastInsertedId()
    {
        AppendLastInsertedId = true;
        return this;
    }
}