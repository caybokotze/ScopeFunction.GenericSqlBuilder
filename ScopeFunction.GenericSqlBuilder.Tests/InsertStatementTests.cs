using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Enums;
using ScopeFunction.GenericSqlBuilder.Exceptions;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
public class InsertStatementTests
{
    [TestFixture]
    public class WithGenericBuilder
    {
        [TestFixture]
        public class WithOptions
        {
            [TestFixture]
            public class WithLastInsertedId
            {
                [TestFixture]
                public class ForMySqlVariant
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o =>
                            {
                                o.WithSqlVariant(Variant.MySql);
                                o.WithAppendedLastInsertedId();
                            })
                            .Into("people")
                            .Build();
                        // act
                        const string expected =
                            "INSERT INTO people (`FirstName`, `LastName`, `Age`) VALUES (@FirstName, @LastName, @Age); SELECT LAST_INSERT_ID();";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }
                }

                [TestFixture]
                public class ForMsSqlVariant
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o =>
                            {
                                o.WithSqlVariant(Variant.MsSql);
                                o.WithAppendedLastInsertedId();
                            })
                            .Into("people")
                            .Build();
                        // act
                        const string expected =
                            "INSERT INTO people ([FirstName], [LastName], [Age]) VALUES (@FirstName, @LastName, @Age); SELECT SCOPE_IDENTITY();";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }
                }

                [TestFixture]
                public class ForPostgreSqlVariant
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o =>
                            {
                                o.WithSqlVariant(Variant.PostgreSql);
                                o.WithAppendedLastInsertedId();
                            })
                            .Into("people")
                            .Build();
                        // act
                        const string expected =
                            "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) RETURNING id;";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }
                }
            }

            [TestFixture]
            public class WithInsertOrUpdate
            {
                [TestFixture]
                public class WithUpdateOnDuplicateKeyIgnoredAndAdded
                {
                    [Test]
                    public void ShouldThrow()
                    {
                        // arrange
                        // act
                        // assert
                        Expect(() => new SqlBuilder()
                                .Insert<Person>(o =>
                                {
                                    o.WithUpdateOnDuplicateKey(p => p.FirstName, p => p.Age);
                                    o.WithUpdateOnDuplicateKeyIgnore(p => p.FirstName);
                                })
                                .Into("people")
                                .Build())
                            .To.Throw<InvalidStatementException>()
                            .With.Message.Containing(SqlBuilderErrorConstants.UpdateAndNotUpdateNotAllowed);
                    }
                }

                [TestFixture]
                public class WithPropertiesToUpdate
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o => { o.WithUpdateOnDuplicateKey(p => p.FirstName, p => p.Age); })
                            .Into("people")
                            .Build();

                        // act
                        const string expected =
                            "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE FirstName = @FirstName, Age = @Age";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }

                    [TestFixture]
                    public class WithSnakeCase
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .Insert<Person>(o =>
                                {
                                    o.WithUpdateOnDuplicateKey(p => p.FirstName, p => p.Age);
                                    o.WithSqlVariant(Variant.MySql);
                                    o.WithPropertyCasing(Casing.SnakeCase);
                                })
                                .Into("people")
                                .Build();

                            // act
                            const string expected =
                                "INSERT INTO people (`first_name`, `last_name`, `age`) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE `first_name` = @FirstName, `age` = @Age;";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }
                }

                [TestFixture]
                public class WithPropertiesToNotUpdate
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o => { o.WithUpdateOnDuplicateKeyIgnore(p => p.Age); })
                            .Into("people")
                            .Build();

                        // act
                        const string expected =
                            "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE FirstName = @FirstName, LastName = @LastName";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }

                    [TestFixture]
                    public class WithSnakeCase
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .Insert<Person>(o =>
                                {
                                    o.WithUpdateOnDuplicateKeyIgnore(p => p.Age);
                                    o.WithSqlVariant(Variant.MySql);
                                    o.WithPropertyCasing(Casing.SnakeCase);
                                })
                                .Into("people")
                                .Build();

                            // act
                            const string expected =
                                "INSERT INTO people (`first_name`, `last_name`, `age`) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE `first_name` = @FirstName, `last_name` = @LastName;";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }
                }

                [TestFixture]
                public class WithAddedProperties
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o =>
                            {
                                o.WithProperty(nameof(Manager.RoleId));
                                o.WithSqlVariant(Variant.MySql);
                                o.WithPropertyCasing(Casing.SnakeCase);
                            })
                            .Into("people")
                            .Build();

                        // act
                        const string expected =
                            "INSERT INTO people (`first_name`, `last_name`, `age`, `role_id`) VALUES (@FirstName, @LastName, @Age, @RoleId);";

                        // assert
                        Expect(sql).To.Equal(expected);
                    }
                }

                [TestFixture]
                public class WithRemovedProperties
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .Insert<Person>(o =>
                            {
                                o.WithoutProperty(nameof(Person.FirstName));
                                o.WithSqlVariant(Variant.MySql);
                                o.WithPropertyCasing(Casing.SnakeCase);
                            })
                            .Into("people")
                            .Build();

                        // act
                        const string expected =
                            "INSERT INTO people (`last_name`, `age`) VALUES (@LastName, @Age);";

                        // assert
                        Expect(sql).To.Equal(expected);
                    }
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
            [TestFixture]
            public class WithOnDuplicateKey
            {
                [Test]
                public void ShouldReturnExpectedResult()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Insert(new[]
                        {
                            nameof(Person.FirstName),
                            nameof(Person.LastName),
                            nameof(Person.Age)
                        }, o => { o.WithUpdateOnDuplicateKey(nameof(Person.FirstName), nameof(Person.LastName)); })
                        .Into("people")
                        .Build();

                    // act
                    const string expected =
                        "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE FirstName = @FirstName, LastName = @LastName";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }


            [TestFixture]
            public class WithAddedProperties
            {
            }

            [TestFixture]
            public class WithRemovedProperties
            {
            }

            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Insert(new[]
                    {
                        nameof(Person.FirstName),
                        nameof(Person.LastName),
                        nameof(Person.Age)
                    }, o => { o.WithUpdateOnDuplicateKey(nameof(Person.FirstName), nameof(Person.LastName)); })
                    .Into("people")
                    .Build();

                // act
                const string expected =
                    "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age) ON DUPLICATE KEY UPDATE FirstName = @FirstName, LastName = @LastName";
                // assert
                Expect(sql).To.Equal(expected);
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
                    .Insert(nameof(Person.FirstName), nameof(Person.LastName), nameof(Person.Age))
                    .Into("people")
                    .Build();
                // act
                const string expected =
                    "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age)";
                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }
}