using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Enums;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
public class UpdateStatementTests
{
    [TestFixture]
    public class WithGenericBuilder
    {
        [TestFixture]
        public class WithOptions
        {
            [TestFixture]
            public class WithAppendProperty
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people", o => o.WithProperty("Foo"))
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET FirstName = @FirstName, LastName = @LastName, Age = @Age, Foo = @Foo WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }

            [TestFixture]
            public class WithoutProperty
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people", o => o.WithoutProperty("Foo"))
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }

            [TestFixture]
            public class WithUppercaseProperty
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people", o => o.WithPropertyCasing(Casing.UpperCase))
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET FIRSTNAME = @FirstName, LASTNAME = @LastName, AGE = @Age WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }

            [TestFixture]
            public class WithSnakeCaseProperty
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people", o => o.WithPropertyCasing(Casing.SnakeCase))
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET first_name = @FirstName, last_name = @LastName, age = @Age WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }

            [TestFixture]
            public class WithMultipleOptions
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people", o =>
                        {
                            o.WithPropertyCasing(Casing.SnakeCase);
                            o.WithProperty("Foo");
                            o.WithoutProperty("Age");
                        })
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET first_name = @FirstName, last_name = @LastName, foo = @Foo WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }
        }

        [TestFixture]
        public class WithoutOptions
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update<Person>("people")
                    .Set()
                    .Where("Id = 21")
                    .Build();
                // act
                const string expected = "UPDATE people SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE Id = 21";
                // assert
                Expect(sql).To.Equal(expected);
            }

            [TestFixture]
            public class WithProvidedGlobalOptions
            {
                [SetUp]
                public void Setup()
                {
                    GenericSqlBuilder.Configure(c =>
                    {
                        c.WithDefaultPropertyCase(Casing.SnakeCase);
                        c.WithDefaultSqlVariant(Variant.MySql);
                    });
                }

                [TearDown]
                public void Teardown()
                {
                    GenericSqlBuilder.Configure(c =>
                    {
                        c.WithDefaultPropertyCase(Casing.Default);
                        c.WithDefaultSqlVariant(Variant.Default);
                    });
                }
                
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Update<Person>("people")
                        .Set()
                        .Where("Id = 21")
                        .Build();
                    // act
                    const string expected = "UPDATE people SET `first_name` = @FirstName, `last_name` = @LastName, `age` = @Age WHERE Id = 21";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }
        }
    }

    [TestFixture]
    public class WithoutGenericBuilder
    {
        [TestFixture]
        public class WithOptions
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                
                // act
                // assert
            }

            [TestFixture]
            public class WithWhereCondition
            {
                
            }
        }

        [TestFixture]
        public class WithoutOptions
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update("people")
                    .Set("FirstName = @FirstName")
                    .Where("Id", e => e.EqualsNumber(12))
                    .And("FirstName", e => e.EqualsString("John"))
                    .Or("LastName", e => e.EqualsString("Williams"))
                    .Build();
                // act
                const string expected = "UPDATE people SET FirstName = @FirstName WHERE Id = 12 AND FirstName = 'John' OR LastName = 'Williams'";
                // assert
                Expect(sql).To.Equal(expected);
            }

            [TestFixture]
            public class WithWhereCondition
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }
        }
    }
}