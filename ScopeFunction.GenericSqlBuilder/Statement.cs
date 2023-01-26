using ScopeFunction.GenericSqlBuilder.Enums;
using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder;

public interface IBuildable
{
    string Build();
}

public enum StatementType
{
    Select,
    Insert,
    Update,
    Delete
}
    
public class Statement
{
    private readonly List<string> _statements;
    private readonly SelectOptions _selectOptions;
    private readonly InsertOptions _insertOptions;
    private readonly IUpdateOptions _updateOptions;
    private readonly StatementType _statementType;

    public Statement(string initial)
    {
        _statements = new List<string>();
        _selectOptions = new SelectOptions();
        _insertOptions = new InsertOptions();
        _updateOptions = new UpdateOptions();
        AddStatement(initial);
    }

    protected Statement(Statement statement, IUpdateOptions options)
    {
        _statementType = StatementType.Update;
        _selectOptions = new SelectOptions();
        _insertOptions = new InsertOptions();
        _statements = statement._statements;

        if (options is not UpdateOptions uo)
        {
            throw new InvalidCastException(Errors.InsertOptionCastException);
        }

        _updateOptions = uo;
    }

    protected Statement(Statement statement, IInsertOptions options)
    {
        _statementType = StatementType.Insert;
        _selectOptions = new SelectOptions();
        _updateOptions = new UpdateOptions();
        _statements = statement._statements;
        
        if (options is not InsertOptions io)
        {
            throw new InvalidCastException(Errors.InsertOptionCastException);
        }

        _insertOptions = io;
    }

    protected Statement(Statement statement, ISelectOptions options)
    {
        _statementType = StatementType.Select;
        _insertOptions = new InsertOptions();
        _updateOptions = new UpdateOptions();
        _statements = statement._statements;
        
        if (options is not SelectOptions so)
        {
            throw new InvalidCastException(Errors.SelectOptionCastException);
        }
        
        _selectOptions = so;
    }
        
    protected void AddStatement(string statement)
    {
        _statements.Add(statement);
    }
    
    protected void RemoveLast()
    {
        _statements.RemoveAt(_statements.Count-1);
    }

    protected void TrimLast()
    {
        var lastStatement = _statements[^1];
        _statements[^1] = lastStatement.Trim();
    }

    public override string ToString()
    {
        return BuildStatement();
    }

    protected string BuildStatement()
    {
        if (_statementType == StatementType.Select
            && _selectOptions.Variant is Variant.MySql or Variant.MsSql)
        {
            TrimLast();
            AddStatement(";");
        }
        
        var statement = StatementBuilder.Build(_statements);

        return statement;
    }
}