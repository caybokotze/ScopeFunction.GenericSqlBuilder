namespace ScopeFunction.GenericSqlBuilder;

public class Finalise : Statement
{
    private readonly ISelectOptions _selectOptions;
    private readonly IInsertOptions _insertOptions;
    private readonly IUpdateOptions _updateOptions;

    public Finalise(Statement statement, ISelectOptions selectOptions) : base(statement)
    {
        _selectOptions = selectOptions;
        _insertOptions = new InsertOptions();
        _updateOptions = new UpdateOptions();
    }

    public Finalise(Statement statement, IInsertOptions insertOptions) : base(statement)
    {
        _insertOptions = insertOptions;
        _selectOptions = new SelectOptions();
        _updateOptions = new UpdateOptions();
    }

    public Finalise(Statement statement, IUpdateOptions updateOptions) : base(statement)
    {
        _updateOptions = updateOptions;
        _insertOptions = new InsertOptions();
        _selectOptions = new SelectOptions();
    }

    public Finalise Append(string clause)
    {
        AddStatement($"{clause} ");
        return this;
    }
    
    public string Build()
    {
        SetInsertOptions(_insertOptions);
        SetUpdateOptions(_updateOptions);
        SetSelectOptions(_selectOptions);
        
        return BuildStatement();
    }
}