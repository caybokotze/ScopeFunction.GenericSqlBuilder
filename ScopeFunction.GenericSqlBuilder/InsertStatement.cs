namespace ScopeFunction.GenericSqlBuilder;

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
        AddStatement($"{table} ");
        return new Finalise(_statement, _options);
    }
}