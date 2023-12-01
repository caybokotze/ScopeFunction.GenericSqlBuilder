using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Enums;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
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
            .Where("age = 21", true)
            .Build();
        
        var statement2 = new SqlBuilder()
            .Select(new []
            {
                nameof(Person.FirstName), 
                nameof(Person.LastName)
            }, o => o.WithPropertyCasing(Casing.SnakeCase))
            .From("people")
            .Where<Person>(f => nameof(f.Age), o => o.Equals("21"))
            .Build();
        
        var statement3 = new SqlBuilder()
            .Select(new []
            {
                nameof(Person.FirstName), 
                nameof(Person.LastName)
            })
            .From("people")
            .Where("age = 21", true)
            .Build();
        
        // act
        const string expected = "SELECT people.firstName, people.lastName FROM people WHERE people.age = 21";
        const string expectedS2 = "SELECT people.first_name, people.last_name FROM people WHERE people.age = 21";
        
        // assert
        Expect(statement).To.Equal(expected);
        Expect(statement2).To.Equal(expectedS2);
        Expect(statement3).To.Equal(expected);
        
        GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.Default));
    }
}