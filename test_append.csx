#r "ScopeFunction.GenericSqlBuilder/bin/Debug/netstandard2.1/ScopeFunction.GenericSqlBuilder.dll"
using ScopeFunction.GenericSqlBuilder;

var sql = new SqlBuilder()
    .Select(new[] { "The quick brown fox" })
    .Append("jumps over the lazy dog")
    .From("table")
    .Build();

Console.WriteLine($"Result: '{sql}'");
Console.WriteLine();
Console.WriteLine("Expected: 'SELECT The quick brown fox jumps over the lazy dog FROM table'");
