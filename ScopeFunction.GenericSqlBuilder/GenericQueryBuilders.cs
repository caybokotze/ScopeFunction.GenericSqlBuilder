using ScopeFunction.GenericSqlBuilder.Enums;

namespace ScopeFunction.GenericSqlBuilder;

public static class GenericSqlBuilder
{
    public static string Query(Func<SqlBuilder, Statement> builder)
    {
        var sqlBuilder = new SqlBuilder();
        var statement = builder(sqlBuilder);
        return statement.Build();
    }
    
    public static void Configure(Action<IConfiguration> configuration)
    {
        var configurationInstance = new Configuration();
        configuration(configurationInstance);
        GenericQueryBuilderSettings.Configuration = configurationInstance;
    }
}

internal static class GenericQueryBuilderSettings
{
    static GenericQueryBuilderSettings()
    {
        Configuration = new Configuration();
    }
    
    public static Configuration Configuration { get; set; }
}

public interface IConfiguration
{
    IConfiguration WithDefaultPropertyCase(Casing casing);
    IConfiguration WithDefaultSqlVariant(Variant variant);
}

internal class Configuration : IConfiguration
{
    public Casing Casing { get; private set; }
    public Variant Variant { get; private set; }
    
    public IConfiguration WithDefaultPropertyCase(Casing casing)
    {
        Casing = casing;
        return this;
    }

    public IConfiguration WithDefaultSqlVariant(Variant variant)
    {
        Variant = variant;
        return this;
    }
}