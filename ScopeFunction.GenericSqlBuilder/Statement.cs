using ScopeFunction.GenericSqlBuilder.Exceptions;

namespace ScopeFunction.GenericSqlBuilder;

public interface IBuilder
{
    string Build();
}
    
public class Statement : IBuilder
{
    private readonly List<string> _statements;
    private readonly SelectOptions _selectOptions;

    protected Statement()
    {
        _statements = new List<string>();
        _selectOptions = new SelectOptions();
    }
    
    public Statement(string initial)
    {
        _statements = new List<string>();
        _selectOptions = new SelectOptions();
        AddStatement(initial);
    }
    
    // public Statement(string initial, Statement? statement)
    // {
    //     _statements = new List<string>();
    //     _selectOptions = new SelectOptions();
    //     AddStatement(initial);
    // }
    
    protected Statement(Statement statement, ISelectOptions selectOptions)
    {
        _statements = statement._statements;
        if (selectOptions is not SelectOptions so)
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
    
    public string Build()
    {
        var statement = StatementBuilder.Build(_statements);
        
        
        
        return statement;
    }
}