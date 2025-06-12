namespace ScopeFunction.GenericSqlBuilder;

public class SqlBuilderErrorConstants
{
    public const string OrAndAndNotAllowed = "When building up a WHERE clause, the options 'HasOrSeparator' and 'HasAndSeparator' can not be used in conjunction";
    public const string PrefixAndNoPrefixNotAllowed = "When building up a WHERE clause, you can not use .WithPrefix() and .WithoutPrefix() simultaneously";
    public const string SelectOptionCastException = "The select options can not be cast from the ISelectOptions interface to a instance of type (T)";
    public const string InsertOptionCastException = "The insert options can not be cast from the IInsertOptions interface to a instance of type (T)";
    public const string UpdateOptionCastException = "The update options can not be cast from the IUpdateOptions interface to a instance of type (T)";
    public const string CosmosDbLastInsertNotSupported = "CosmosDb does not support the LAST INSERT ID option. Use another SQL variant OR remove that option";
    public const string UpdateAndNotUpdateNotAllowed = "Using the .WithUpdateOnDuplicateKey extension and the WithUpdateOnDuplicateKeyIgnore extension simultaneously is not allowed.";
}