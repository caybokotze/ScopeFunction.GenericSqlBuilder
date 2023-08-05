using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;
using static ScopeFunction.GenericSqlBuilder.GenericSqlBuilder;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
public class StaticQueryBuilderTests
{
    [Test]
    public void ShouldReturnExpectedQuery()
    {
        // arrange
        var builderSql = new SqlBuilder()
            .SelectAll(o => o.WithPropertyPrefix("p"))
            .From("people")
            .Where<Person>(p => nameof(p.Age), w => w.EqualsNumber(50))
            .OrderBy<Person>(o => nameof(o.Age))
            .Build();

        var staticBuilderSql =
            Query(q => q.SelectAll(o => o.WithPropertyPrefix("p"))
                .From("people")
                .Where<Person>(p => nameof(p.Age), w => w.EqualsNumber(50))
                .OrderBy<Person>(o => nameof(o.Age))
            );
        
        // act
        const string expected = "SELECT * FROM people WHERE p.Age = 50 ORDER BY p.Age ASC";
        // assert
        Expect(builderSql).To.Equal(expected);
        Expect(staticBuilderSql).To.Equal(expected);
        Expect(builderSql).To.Equal(builderSql);
    }
}