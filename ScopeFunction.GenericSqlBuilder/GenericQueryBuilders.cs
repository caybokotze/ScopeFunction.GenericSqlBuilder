using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public static class GenericSqlBuilder
{
    public static string Query(Func<SqlBuilder, Statement> builder)
    {
        var sqlBuilder = new SqlBuilder();
        var statement = builder(sqlBuilder);
        return statement.ToString();
    }
    
    public static void Configure(Action<IGenericSqlBuilderConfiguration> configuration)
    {
        var configurationInstance = new GenericSqlBuilderConfiguration();
        configuration(configurationInstance);
        GenericQueryBuilderSettings.GenericSqlBuilderConfiguration = configurationInstance;
    }
}

internal static class GenericQueryBuilderSettings
{
    static GenericQueryBuilderSettings()
    {
        GenericSqlBuilderConfiguration = new GenericSqlBuilderConfiguration();
    }
    
    public static GenericSqlBuilderConfiguration GenericSqlBuilderConfiguration { internal get; set; }
}

public interface IGenericSqlBuilderConfiguration
{
    IGenericSqlBuilderConfiguration WithDefaultPropertyCase(Casing casing);
    IGenericSqlBuilderConfiguration WithDefaultSqlVariant(Variant variant);
}

internal class GenericSqlBuilderConfiguration : IGenericSqlBuilderConfiguration
{
    public Casing Casing { get; private set; }
    public Variant Variant { get; private set; }
    
    public IGenericSqlBuilderConfiguration WithDefaultPropertyCase(Casing casing)
    {
        Casing = casing;
        return this;
    }

    public IGenericSqlBuilderConfiguration WithDefaultSqlVariant(Variant variant)
    {
        Variant = variant;
        return this;
    }
}