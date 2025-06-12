using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Enums;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class OptionsTesting
{
    public class WithSelectStatements
    {
        [Test]
        public void ShouldPersistAcrossStatements()
        {
            // arrange
            GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.CamelCase));

            var statement = new SqlBuilder()
                .Select(new[]
                {
                    nameof(Person.FirstName),
                    nameof(Person.LastName)
                })
                .From("people")
                .Where("age = 21", true)
                .Build();

            var statement2 = new SqlBuilder()
                .Select(new[]
                {
                    nameof(Person.FirstName),
                    nameof(Person.LastName)
                }, o => o.WithPropertyCasing(Casing.SnakeCase))
                .From("people")
                .Where<Person>(f => nameof(f.Age), o => o.Equals("21"))
                .Build();

            var statement3 = new SqlBuilder()
                .Select(new[]
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

        [TestFixture]
        public class WithGenericBuilder
        {
            [Test]
            public void ShouldPersistAcrossStatements()
            {
                // arrange
                GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.CamelCase));

                var statement = new SqlBuilder()
                    .Select<Person>()
                    .From("people")
                    .Where("age = 21", true)
                    .Build();

                var statement2 = new SqlBuilder()
                    .Select<Person>(o => o.WithPropertyCasing(Casing.SnakeCase))
                    .From("people")
                    .Where<Person>(f => nameof(f.Age), o => o.Equals("21"))
                    .Build();

                var statement3 = new SqlBuilder()
                    .Select<Person>()
                    .From("people")
                    .Where("age = 21", true)
                    .Build();

                // act
                const string expected = "SELECT people.firstName, people.lastName, people.age FROM people WHERE people.age = 21";
                const string expectedS2 =
                    "SELECT people.first_name, people.last_name, people.age FROM people WHERE people.age = 21";

                // assert
                Expect(statement).To.Equal(expected);
                Expect(statement2).To.Equal(expectedS2);
                Expect(statement3).To.Equal(expected);

                GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.Default));
            }
        }
    }

    [TestFixture]
    public class WithInsertStatements
    {
        [Test]
        public void ShouldPersistAcrossStatements()
        {
            // arrange
            GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.CamelCase));

            var statement = new SqlBuilder()
                .Insert<Person>()
                .Into("people")
                .Build();

            var statement2 = new SqlBuilder()
                .Insert<Person>(o => o.WithPropertyCasing(Casing.SnakeCase))
                .Into("people")
                .Build();

            var statement3 = new SqlBuilder()
                .Insert<Person>()
                .Into("people")
                .Build();

            // act
            const string expected =
                "INSERT INTO people (firstName, lastName, age) VALUES (@FirstName, @LastName, @Age)";
            
            const string expectedS2 =
                "INSERT INTO people (first_name, last_name, age) VALUES (@FirstName, @LastName, @Age)";

            // assert
            Expect(statement).To.Equal(expected);
            Expect(statement2).To.Equal(expectedS2);
            Expect(statement3).To.Equal(expected);

            GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.Default));
        }
    }

    public class WithUpdateStatements
    {
        [Test]
        public void ShouldPersistAcrossStatements()
        {
            // arrange
            GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.CamelCase));

            var statement = new SqlBuilder()
                .Update<Person>("people")
                .Set()
                .Where("Id = 21")
                .Build();

            var statement2 = new SqlBuilder()
                .Update<Person>("people", o => o.WithPropertyCasing(Casing.SnakeCase))
                .Set()
                .Where("Id = 21")
                .Build();

            var statement3 = new SqlBuilder()
                .Update<Person>("people")
                .Set()
                .Where("Id = 21")
                .Build();

            // act
            const string expected =
                "UPDATE people SET firstName = @FirstName, lastName = @LastName, age = @Age WHERE Id = 21";
            
            const string expectedS2 =
                "UPDATE people SET first_name = @FirstName, last_name = @LastName, age = @Age WHERE Id = 21";

            // assert
            Expect(statement).To.Equal(expected);
            Expect(statement2).To.Equal(expectedS2);
            Expect(statement3).To.Equal(expected);

            GenericSqlBuilder.Configure(c => c.WithDefaultPropertyCase(Casing.Default));
        }
    }
    
}