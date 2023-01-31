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
                    .Where("Id = 21")
                    .Build();
                // act
                const string expected = "UPDATE people SET FirstName = @FirstName WHERE Id = 21";
                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }
}