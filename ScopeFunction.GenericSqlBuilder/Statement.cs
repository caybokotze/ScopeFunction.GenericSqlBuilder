using System.Net.Http.Headers;
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
    private readonly StatementType _statementType;
    
    private SelectOptions _selectOptions;
    private InsertOptions _insertOptions;
    private UpdateOptions _updateOptions;

    public Statement(string initial, StatementType statementType)
    {
        _statements = new List<string>(512);
        _statementType = statementType;
        _selectOptions = new SelectOptions();
        _insertOptions = new InsertOptions();
        _updateOptions = new UpdateOptions();
        AddStatement(initial);
    }

    protected Statement(Statement statement)
    {
        _statementType = statement._statementType;
        _selectOptions = statement._selectOptions;
        _insertOptions = statement._insertOptions;
        _updateOptions = statement._updateOptions;
        _statements = statement._statements;
    }

    protected void SetInsertOptions(IInsertOptions options)
    {
        if (options is not InsertOptions io)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.InsertOptionCastException);
        }
        
        _insertOptions = io;
    }

    protected void SetUpdateOptions(IUpdateOptions options)
    {
        if (options is not UpdateOptions uo)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.InsertOptionCastException);
        }
        
        _updateOptions = uo;
    }

    protected void SetSelectOptions(ISelectOptions options)
    {
        
        if (options is not SelectOptions so)
        {
            throw new InvalidCastException(SqlBuilderErrorConstants.InsertOptionCastException);
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

    private void TrimLast(bool trimTrailingComma = false)
    {
        _statements[^1] = _statements[^1].Trim();

        if (trimTrailingComma)
        {
            _statements[^1] = _statements[^1].TrimEnd(',');
        }
    }

    public override string ToString()
    {
        return BuildStatement();
    }

    private void ApplySelectStatementVariance()
    {
        switch (_selectOptions.Variant)
        {
            case Variant.MsSql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.MySql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.PostgreSql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.CosmosDb:
                break;
            case Variant.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_selectOptions.Variant));
        }
    }

    private void ApplyInsertStatementVariance()
    {
        switch (_insertOptions.Variant)
        {
            case Variant.MsSql:
                TrimLast();
                AddStatement(";");
                if (_insertOptions.AppendLastInsertedId)
                {
                    AddStatement(" ");
                    AddStatement("SELECT SCOPE_IDENTITY();");
                }
                break;
            case Variant.MySql:
                TrimLast();
                AddStatement(";");
                if (_insertOptions.AppendLastInsertedId)
                {
                    AddStatement(" ");
                    AddStatement("SELECT LAST_INSERT_ID();");
                }
                break;
            case Variant.PostgreSql:
                TrimLast();
                if (_insertOptions.AppendLastInsertedId)
                {
                    AddStatement(" ");
                    AddStatement("RETURNING id;");
                }
                break;
            case Variant.CosmosDb:
                break;
            case Variant.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_selectOptions.Variant));
        }
    }

    private void ApplyUpdateStatementVariance()
    {
        switch (_updateOptions.Variant)
        {
            case Variant.MsSql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.MySql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.PostgreSql:
                TrimLast();
                AddStatement(";");
                break;
            case Variant.CosmosDb:
                break;
            case Variant.Default:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_selectOptions.Variant));
            
        }
    }

    protected string BuildStatement()
    {
        if (_statementType == StatementType.Select)
        {
            ApplySelectStatementVariance();
        }
        
        if (_statementType == StatementType.Update)
        {
            ApplyUpdateStatementVariance();
        }
        
        if (_statementType == StatementType.Insert)
        {
            ApplyInsertStatementVariance();
        }
        
        var statement = StatementBuilder.Build(_statements);

        return statement;
    }
}