using NUnit.Framework;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
public class OptionsTesting
{
    [Test]
    public void ShouldPersistAcrossStatements()
    {
        // arrange
        GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.CamelCase));
        
        var statement = new SqlBuilder()
            .Select(new []
            {
                nameof(Person.FirstName), 
                nameof(Person.LastName)
            })
            .From("people")
            .Where("people.Age = 21")
            .Build();
        // act
        const string expected = "";
        // assert
    }
}