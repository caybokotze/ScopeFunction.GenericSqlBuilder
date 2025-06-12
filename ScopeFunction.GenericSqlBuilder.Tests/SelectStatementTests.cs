using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Enums;
using ScopeFunction.GenericSqlBuilder.Exceptions;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class SelectStatementTests
{
    [TestFixture]
    public class Select
    {
        [TestFixture]
        public class WithGenericReflectiveBuilder
        {
            [TestFixture]
            public class WithSelectOptions
            {
                [TestFixture]
                public class WithWhereClause
                {
                    [TestFixture]
                    public class WithWhereOptions
                    {
                        [TestFixture]
                        public class WithWherePrefix
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>(s =>
                                    {
                                        s.WithoutProperty(nameof(Person.Age));
                                    })
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18"
                                    }, o => o.WithPropertyPrefix("w"))
                                    .Build();
                                // act
                                const string expected = "SELECT c.FirstName, c.LastName FROM c WHERE w.Age = 18";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class WithSeparators
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18",
                                        $"{nameof(p.Age)} = 20"
                                    }, w => w.WithAndSeparator())
                                    .Build();
                                // act
                                const string expected =
                                    "SELECT c.FirstName, c.LastName, c.Age FROM c WHERE c.Age = 18 AND c.Age = 20";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class WithAppendedWhere
                        {
                            [Test]
                            public void ShouldAppendWhereWithAndStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>(o =>
                                    {
                                        o.WithPropertyPrefix("p");
                                    })
                                    .From("people p")
                                    .Where("p.Id = 12")
                                    .AppendWhere(w => w.Where("p.Age = @Age", o => o.WithAndSeparator()))
                                    .Build();
                                
                                // act
                                var expected =
                                    "SELECT p.FirstName, p.LastName, p.Age FROM people p WHERE p.Id = 12 AND p.Age = @Age";
                                
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }
                        
                        [TestFixture]
                        public class WithAppendedWhereAndNestedWhere
                        {
                            [Test]
                            public void ShouldReturnExpectedQuery()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .Select("o.id")
                                    .From("business_orders o")
                                    .LeftJoin("business_customers c")
                                    .On("c.id = o.customer_id")
                                    .Where("o.business_id = @BusinessId")
                                    .Append("AND")
                                    .AppendWhereIf(() => true, f =>
                                        f.NestedWhere(o =>
                                        {
                                            o.Where("c.email LIKE @Filter", w => w.WithoutSeparator());
                                        }))
                                    .Build();
                                
                                // act
                                var expected = "SELECT o.id FROM business_orders o LEFT JOIN business_customers c ON c.id = o.customer_id WHERE o.business_id = @BusinessId AND ( c.email LIKE @Filter )";
                                // assert
                                Expect(expected).To.Equal(sql);
                            }
                        }
                    }

                    [TestFixture]
                    public class WithAppendedLeftJoin
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("c")
                                .LeftJoin("d")
                                .On("d.c_id = c.id")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithAndSeparator())
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c LEFT JOIN d ON d.c_id = c.id WHERE c.Age = 18 AND c.Age = 20";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAppendedRightJoin
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("c")
                                .RightJoin("d")
                                .On("d.c_id = c.id")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithAndSeparator())
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c RIGHT JOIN d ON d.c_id = c.id WHERE c.Age = 18 AND c.Age = 20";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAppendedInnerJoin
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("c")
                                .InnerJoin("d")
                                .On("d.c_id = c.id")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithAndSeparator())
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c INNER JOIN d ON d.c_id = c.id WHERE c.Age = 18 AND c.Age = 20";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                    
                    [TestFixture]
                    public class WithAppendedJoin
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("c")
                                .Join("d")
                                .On("d.c_id = c.id")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithAndSeparator())
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c JOIN d ON d.c_id = c.id WHERE c.Age = 18 AND c.Age = 20";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                }

                [TestFixture]
                public class WithoutWhereClause
                {
                    [TestFixture]
                    public class WithSpecifiedSplitOn
                    {
                        [Test]
                        public void ShouldAddSplitOnToProvidedProperty()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>(o =>
                                {
                                    o.WithSplitOn(nameof(Person.Age));
                                    o.WithPropertyPrefix("p");
                                })
                                .AppendSelect(a =>
                                {
                                    a.Select<Car>(o => o.WithPropertyPrefix("c"));
                                })
                                .From("p")
                                .LeftJoin("c")
                                .On("c.id = p.car_id")
                                .Build();
                            // act
                            const string expected = "SELECT p.Age, p.FirstName, p.LastName, c.Age, c.Make, c.Model, c.Year FROM p LEFT JOIN c ON c.id = p.car_id";
                            // assert
                            Expect(expected).To.Equal(sut);
                        }
                    }
                    
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sut = new SqlBuilder()
                            .Select<Person>()
                            .From("p")
                            .Build();
                        // act
                        const string expected = "SELECT p.FirstName, p.LastName, p.Age FROM p";
                        // assert
                        Expect(expected).To.Equal(sut);
                    }

                    [TestFixture]
                    public class WithAppendedJoins
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("p")
                                .LeftJoin("q")
                                .On("q.p_id = p.id")
                                .InnerJoin("o")
                                .On("o.q_id = q.id")
                                .RightJoin("r")
                                .On("r.o_id = o.id")
                                .Join("u")
                                .On("u.r_id = r.id")
                                .Build();
                            // act
                            const string expected = "SELECT p.FirstName, p.LastName, p.Age FROM p LEFT JOIN q ON q.p_id = p.id INNER JOIN o ON o.q_id = q.id RIGHT JOIN r ON r.o_id = o.id JOIN u ON u.r_id = r.id";
                            // assert
                            Expect(expected).To.Equal(sut);
                        }
                    }
                }


                [TestFixture]
                public class WithAppendSelect
                {
                    [TestFixture]
                    public class WithSplitOnOption
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>(o =>
                                {
                                    o.WithSplitOn(nameof(Person.Age));
                                })
                                .From("p")
                                .Build();
                            // act
                            const string expected = "SELECT p.Age, p.FirstName, p.LastName FROM p";
                            // assert
                            Expect(expected).To.Equal(sut);
                        }
                    }
                    
                    [Test]
                    public void ShouldReturnExpectedResult()
                    {
                        // arrange
                        
                        // act
                        // assert
                    }
                }
            }

            [TestFixture]
            public class WithoutSelectOptions
            {
                [TestFixture]
                public class WithWhereClause
                {
                    [TestFixture]
                    public class WithWhereOptions
                    {
                        [TestFixture]
                        public class WithWherePrefix
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18"
                                    }, o => o.WithPropertyPrefix("w"))
                                    .Build();
                                // act
                                const string expected = "SELECT c.FirstName, c.LastName, c.Age FROM c WHERE w.Age = 18";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class WithSeparators
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18",
                                        $"{nameof(p.Age)} = 20"
                                    })
                                    .Build();
                                // act
                                const string expected =
                                    "SELECT c.FirstName, c.LastName, c.Age FROM c WHERE c.Age = 18 OR c.Age = 20";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }
                    }

                    [TestFixture]
                    public class WithAppendedJoin
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>()
                                .From("p")
                                .LeftJoin("q")
                                .On("q.p_id = p.id")
                                .InnerJoin("o")
                                .On("o.q_id = q.id")
                                .RightJoin("r")
                                .On("r.o_id = o.id")
                                .Where<Person>(p => new []{$"{nameof(p.Age)} = 25"})
                                .Build();
                            // act
                            const string expected = "SELECT p.FirstName, p.LastName, p.Age FROM p LEFT JOIN q ON q.p_id = p.id INNER JOIN o ON o.q_id = q.id RIGHT JOIN r ON r.o_id = o.id WHERE p.Age = 25";
                            // assert
                            Expect(expected).To.Equal(sut);
                        }
                    }
                }
            }
        }

        [TestFixture]
        public class WithoutGenericReflectiveBuilder
        {
            [TestFixture]
            public class WithGenericParameter
            {
                [TestFixture]
                public class WithSelectOptions
                {
                    [TestFixture]
                    public class WithWhereClause
                    {
                        [TestFixture]
                        public class WithTypedSelect
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>(s => new []
                                    {
                                        nameof(s.FirstName),
                                        nameof(s.LastName),
                                        nameof(s.Age)
                                    }, s => s.WithTop(10).WithPropertyPrefix("p"))
                                    .From("people as p")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18",
                                        $"{nameof(p.Age)} = 20"
                                    })
                                    .Build();
                                // act
                                const string expected =
                                    "SELECT TOP 10 p.FirstName, p.LastName, p.Age FROM people as p WHERE p.Age = 18 OR p.Age = 20";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class WithArraySelect
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sut = new SqlBuilder()
                                    .Select<Person>(p => new []
                                    {
                                        nameof(p.FirstName),
                                        nameof(p.LastName),
                                        nameof(p.Age)
                                    }, o => o.WithoutPropertyPrefix())
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)} = 18",
                                        $"{nameof(p.Age)} = 20"
                                    })
                                    .Build();
                                // act
                                const string expected =
                                    "SELECT FirstName, LastName, Age FROM c WHERE Age = 18 OR Age = 20";
                                // assert
                                Expect(sut).To.Equal(expected);
                            }

                            [TestFixture]
                            public class WithNoSelectPrefixAndSeparateWhereClausePrefix
                            {
                                [Test]
                                public void ShouldReturnExpectedWhereClause()
                                {
                                    // arrange
                                    var sut = new SqlBuilder()
                                        .Select<Person>(p => new []
                                        {
                                            nameof(p.FirstName),
                                            nameof(p.LastName),
                                            nameof(p.Age)
                                        }, o => o.WithoutPropertyPrefix())
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.Age)} = 18",
                                            $"{nameof(p.Age)} = 20"
                                        }, w => w.WithPropertyPrefix("w"))
                                        .And()
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} like %John%"
                                        })
                                        .Or()
                                        .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                                        .And()
                                        .Where<Person>(p => nameof(p.LastName), w => w.Like("Williams"))
                                        .Build();
                                    // act
                                    const string expected =
                                        "SELECT FirstName, LastName, Age FROM c WHERE w.Age = 18 OR w.Age = 20 AND FirstName like %John% OR FirstName = 'John' AND LastName LIKE 'Williams'";
                                    // assert
                                    Expect(sut).To.Equal(expected);
                                }
                            }
                        }
                    }

                    [TestFixture]
                    public class WithoutWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>(p => new[]
                                {
                                    nameof(p.FirstName),
                                    nameof(p.LastName),
                                    nameof(p.Age)
                                }, o => o.WithPropertyPrefix("d"))
                                .From("c")
                                .Build();
                            // act
                            const string expected =
                                "SELECT d.FirstName, d.LastName, d.Age FROM c";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                }

                [TestFixture]
                public class WithoutSelectOptions
                {
                    [TestFixture]
                    public class WithWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>(p => new []
                                {
                                    nameof(p.FirstName),
                                    nameof(p.LastName),
                                    nameof(p.Age)
                                })
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithPropertyPrefix("w"))
                                .And()
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} like %John%"
                                })
                                .Or()
                                .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                                .And()
                                .Where<Person>(p => nameof(p.LastName), w => w.Like("Williams"))
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c WHERE w.Age = 18 OR w.Age = 20 AND c.FirstName like %John% OR c.FirstName = 'John' AND c.LastName LIKE 'Williams'";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithoutWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select<Person>(p => new[]
                                {
                                    nameof(p.FirstName),
                                    nameof(p.LastName),
                                    nameof(p.Age)
                                })
                                .From("c")
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM c";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                }
            }

            [TestFixture]
            public class WithoutGenericParameter
            {
                [TestFixture]
                public class WithSelectOptions
                {
                    [TestFixture]
                    public class WithWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select(new []
                                {
                                    nameof(Person.FirstName),
                                    nameof(Person.LastName),
                                    nameof(Person.Age)
                                }, s => s.WithPropertyPrefix("c"))
                                .From("people as c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithPropertyPrefix("w"))
                                .And()
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} like %John%"
                                })
                                .Or()
                                .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                                .And()
                                .Where<Person>(p => nameof(p.LastName), w => w.Like("Williams"))
                                .Build();
                            // act
                            const string expected =
                                "SELECT c.FirstName, c.LastName, c.Age FROM people as c WHERE w.Age = 18 OR w.Age = 20 AND c.FirstName like %John% OR c.FirstName = 'John' AND c.LastName LIKE 'Williams'";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithoutWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select(new[]
                                {
                                    nameof(Person.FirstName),
                                    nameof(Person.LastName),
                                    nameof(Person.Age)
                                }, o => o.WithPropertyPrefix("p"))
                                .From("people as p")
                                .Build();
                            // act
                            const string expected =
                                "SELECT p.FirstName, p.LastName, p.Age FROM people as p";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                }


                [TestFixture]
                public class WithoutSelectOptions
                {
                    [TestFixture]
                    public class WithWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            var sut = new SqlBuilder()
                                .Select(new[]
                                {
                                    nameof(Person.FirstName),
                                    nameof(Person.LastName),
                                    nameof(Person.Age)
                                })
                                .From("people")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.Age)} = 18",
                                    $"{nameof(p.Age)} = 20"
                                }, w => w.WithPropertyPrefix("w"))
                                .And()
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} like %John%"
                                })
                                .Build();
                            // act
                            const string expected =
                                "SELECT people.FirstName, people.LastName, people.Age FROM people WHERE w.Age = 18 OR w.Age = 20 AND people.FirstName like %John%";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithoutWhereClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sut = new SqlBuilder()
                                .Select(new[]
                                {
                                    nameof(Person.FirstName),
                                    nameof(Person.LastName),
                                    nameof(Person.Age)
                                })
                                .From("people")
                                .Build();
                            // act
                            const string expected =
                                "SELECT people.FirstName, people.LastName, people.Age FROM people";
                            // assert
                            Expect(sut).To.Equal(expected);
                        }
                    }
                }
            }
        }

        [TestFixture]
        public class WithAppendSelect
        {
            [TestFixture]
            public class WithSinglePrefix
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Select<Person>(s => s.WithoutPropertyPrefix())
                        .AppendSelect(a =>
                            a.Select<Manager>(s => s.WithoutPropertyPrefix()))
                        .From("people")
                        .Where<Person>(p => nameof(p.Age), w => w.EqualsNumber(50))
                        .Build();
                    // act
                    const string expected = "SELECT FirstName, LastName, Age, RoleId FROM people WHERE Age = 50";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }

            [TestFixture]
            public class WithMultipleDifferentPrefixes
            {
                [Test]
                public void ShouldReturnExpectedStatement()
                {
                    // arrange
                    var sql = new SqlBuilder()
                        .Select<Person>(s => s.WithPropertyPrefix("a"))
                        .AppendSelect(a =>
                            a.Select<Manager>(s => s.WithPropertyPrefix("b")))
                        .From("people")
                        .Where<Person>(p => nameof(p.Age), w =>
                        {
                            w.EqualsNumber(50);
                        })
                        .Build();
                    // act
                    const string expected = "SELECT a.FirstName, a.LastName, a.Age, b.RoleId FROM people WHERE b.Age = 50";
                    // assert
                    Expect(sql).To.Equal(expected);
                }
            }
        }

        [TestFixture]
        public class WithAppendSelectAndAppendStatements
        {
            [Test]
            public void ShouldReturnExpectedResult()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Person>(o =>
                    {
                        o.WithPropertyPrefix("p");
                        o.WithPropertyCasing(Casing.SnakeCase);
                    })
                    .Append("(SELECT COUNT(*) FROM people) as person_count", true)
                    .AppendSelect(a =>
                        a.Select<Manager>(o => o.WithPropertyPrefix("m")))
                    .From("people")
                    .LeftJoin("managers")
                    .On("p.id = m.person_id")
                    .Where("p.id = @PersonId")
                    .AppendWhere(a =>
                        a.Where("p.date_created BETWEEN DATE(@DateFrom) AND DATE(@DateTo)", o => o.WithAndSeparator()))
                    .AppendWhereIf(() => 5 > 1, a =>
                        a.Where("p.person_status = @PersonStatus", o => o.WithAndSeparator()))
                    .OrderBy("p.date_modified", o => o.WithDesc())
                    .Build();
                // act
                // assert
                Expect(sql).To.Equal("SELECT (SELECT COUNT(*) FROM people) as person_count, p.first_name, p.last_name, p.age, m.role_id FROM people LEFT JOIN managers ON p.id = m.person_id WHERE p.id = @PersonId AND p.date_created BETWEEN DATE(@DateFrom) AND DATE(@DateTo) AND p.person_status = @PersonStatus ORDER BY p.date_modified DESC");
            }
        }

        [TestFixture]
        public class WithCaseConversion
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sut = new SqlBuilder()
                    .Select<Person>(p => new[] {nameof(p.FirstName)}, o => o.WithPropertyPrefix("pe"))
                    .From("people")
                    .Build();
                // act
                // assert
            }

            [TestFixture]
            public class WithAdditiveProperties
            {
                
            }

            [TestFixture]
            public class WithRemovedProperties
            {
                
            }
        }

        [TestFixture]
        public class WithSqlVariants
        {
            [Test]
            public void ShouldReturnExpectedStatements()
            {
                // arrange
                
                // act
                // assert
            }
        }
    }

    [TestFixture]
    public class SelectAll
    {
        [TestFixture]
        public class WithWhereClause
        {
            [TestFixture]
            public class WithoutOptions
            {
                [TestFixture]
                public class StandardWhere
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .SelectAll()
                            .From("Families")
                            .Where("Families.address.state")
                            .In("NY", "WA", "CA", "PA")
                            .Build();
                        // act
                        const string expected =
                            "SELECT * FROM Families WHERE Families.address.state IN (NY, WA, CA, PA)";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }
                }

                [TestFixture]
                public class ArrayWhere
                {
                    [Test]
                    public void ShouldReturnExpectedStatement()
                    {
                        // arrange
                        var sql = new SqlBuilder()
                            .SelectAll()
                            .From("Families")
                            .Where(new[] {"id = 'Wakefield Family'"})
                            .Build();
                        // act
                        const string expected = "SELECT * FROM Families WHERE Families.id = 'Wakefield Family'";
                        // assert
                        Expect(sql).To.Equal(expected);
                    }

                    [TestFixture]
                    public class WithMultipleClauses
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("Families")
                                .Where(new[] {"id = 'Wakefield Family'", "id = 'Martin Family'"})
                                .Build();
                            // act
                            const string expected =
                                "SELECT * FROM Families WHERE Families.id = 'Wakefield Family' OR Families.id = 'Martin Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }
                }

                [TestFixture]
                public class GenericArrayWhere
                {
                    [TestFixture]
                    public class WithSingleClause
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("Families")
                                .Where(new[] {"id = 'Wakefield Family'", "id = 'Martin Family'"})
                                .Build();
                            // act
                            const string expected =
                                "SELECT * FROM Families WHERE Families.id = 'Wakefield Family' OR Families.id = 'Martin Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithMultipleClauses
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("Families")
                                .Where(new[] {"id = 'Wakefield Family'", "id = 'Martin Family'"})
                                .Build();
                            // act
                            const string expected =
                                "SELECT * FROM Families WHERE Families.id = 'Wakefield Family' OR Families.id = 'Martin Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }
                }
            }

            [TestFixture]
            public class WithOptions
            {
                [TestFixture]
                public class ArrayWhere
                {
                    [TestFixture]
                    public class WithoutPrefix
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("Families")
                                .Where(new[] {"id = 'Wakefield Family'"}, w => w.WithoutPropertyPrefix())
                                .Build();
                            // act
                            const string expected = "SELECT * FROM Families WHERE id = 'Wakefield Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithPrefix
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("Families")
                                .Where(new[] {"id = 'Wakefield Family'"}, w => w.WithPropertyPrefix("f"))
                                .Build();
                            // act
                            const string expected = "SELECT * FROM Families WHERE f.id = 'Wakefield Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }

                        [TestFixture]
                        public class WithMultipleWhereStatements
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .SelectAll()
                                    .From("Families")
                                    .Where(new[] {"id = 'Wakefield Family'", "id = 'Martin Family'"},
                                        w => w.WithPropertyPrefix("f"))
                                    .Build();
                                // act
                                const string expected =
                                    "SELECT * FROM Families WHERE f.id = 'Wakefield Family' OR f.id = 'Martin Family'";
                                // assert
                                Expect(sql).To.Equal(expected);
                            }
                        }
                    }

                    [TestFixture]
                    public class WithAndWithoutPrefix
                    {
                        [Test]
                        public void ShouldThrow()
                        {
                            // arrange
                            // act
                            // assert
                            Expect(() => new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where(new[] {"id = 'Wakefield Family'"}, w =>
                                    {
                                        w.WithoutPropertyPrefix();
                                        w.WithPropertyPrefix("c");
                                    })
                                    .Build())
                                .To.Throw<InvalidStatementException>()
                                .With.Message
                                .Containing(SqlBuilderErrorConstants.PrefixAndNoPrefixNotAllowed);
                        }
                    }

                    [TestFixture]
                    public class WithOrSeparator
                    {
                        [Test]
                        public void ShouldSeparateWithOr()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where(new[]
                                {
                                    "id = 'Wakefield Family'",
                                    "id = 'Watson Family'"
                                }, w => { w.WithOrSeparator(); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE c.id = 'Wakefield Family' OR c.id = 'Watson Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAndSeparator
                    {
                        [Test]
                        public void ShouldSeparateWithAnd()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where(new[]
                                {
                                    "id = 'Wakefield Family'",
                                    "id = 'Watson Family'"
                                }, w => { w.WithAndSeparator(); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE c.id = 'Wakefield Family' AND c.id = 'Watson Family'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAndOrSeparator
                    {
                        [Test]
                        public void ShouldThrow()
                        {
                            // arrange
                            // act
                            // assert
                            Expect(() => new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where(new[]
                                    {
                                        "id = 'Wakefield Family'",
                                        "id = 'Watson Family'"
                                    }, w =>
                                    {
                                        w.WithAndSeparator();
                                        w.WithOrSeparator();
                                    })
                                    .Build()).To.Throw<InvalidStatementException>()
                                .With.Message
                                .Containing(
                                    "When building up a WHERE clause, the options 'HasOrSeparator' and 'HasAndSeparator' can not be used in conjunction");
                        }
                    }

                    [TestFixture]
                    public class WithAppend
                    {
                        [TestFixture]
                        public class WithAppendWhereIf
                        {
                            [TestFixture]
                            public class WhenOutcomeIsTrue
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where(new[]
                                        {
                                            $"{nameof(Person.FirstName)} = 'John'",
                                            $"{nameof(Person.LastName)} = 'Watson'"
                                        }, w => { w.WithAndSeparator(); })
                                        .AppendWhere(f =>
                                            f.Where(new[]
                                            {
                                                $"{nameof(Person.LastName)} = 'Fredrick'"
                                            }))
                                        .AppendWhereIf(() => 5 > 1, f =>
                                            f.Where<Person>(p => new[]
                                            {
                                                $"{nameof(p.FirstName)} = 'Jack'"
                                            }, o => o.WithAndSeparator()))
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick' AND c.FirstName = 'Jack'";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WhenOutcomeIsFalse
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where(new[]
                                        {
                                            $"{nameof(Person.FirstName)} = 'John'",
                                            $"{nameof(Person.LastName)} = 'Watson'"
                                        }, w => { w.WithAndSeparator(); })
                                        .AppendWhere(f =>
                                            f.Where(new[]
                                            {
                                                $"{nameof(Person.LastName)} = 'Fredrick'"
                                            }))
                                        .AppendWhereIf(() => 5 > 10, f =>
                                            f.Where<Person>(p => new[]
                                            {
                                                $"{nameof(p.FirstName)} = 'Jack'"
                                            }, o => o.WithAndSeparator()))
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick'";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }
                        }
                        
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where(new[]
                                {
                                    $"{nameof(Person.FirstName)} = 'John'",
                                    $"{nameof(Person.LastName)} = 'Watson'"
                                }, w => { w.WithAndSeparator(); })
                                .AppendWhere(f =>
                                    f.Where(new[]
                                    {
                                        $"{nameof(Person.LastName)} = 'Fredrick'"
                                    }))
                                .Build();
                            // act
                            var expected =
                                "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }

                        [TestFixture]
                        public class WithNewPrefix
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where(new[]
                                    {
                                        $"{nameof(Person.FirstName)} = 'John'",
                                        $"{nameof(Person.LastName)} = 'Watson'"
                                    }, w => { w.WithAndSeparator(); })
                                    .AppendWhere(f =>
                                        f.Where(new[]
                                        {
                                            $"{nameof(Person.LastName)} = 'Fredrick'"
                                        }, w => w.WithPropertyPrefix("a")))
                                    .Build();
                                // act
                                var expected =
                                    "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR a.LastName = 'Fredrick'";
                                // assert
                                Expect(sql).To.Equal(expected);
                            }

                            [TestFixture]
                            public class WithTwoDistinctWherePrefixes
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where(new[]
                                        {
                                            $"{nameof(Person.FirstName)} = 'John'",
                                            $"{nameof(Person.LastName)} = 'Watson'"
                                        }, w => { w.WithAndSeparator(); })
                                        .AppendWhere(f =>
                                            f.Where(new[]
                                            {
                                                $"{nameof(Person.LastName)} = 'Fredrick'"
                                            }, w =>
                                            {
                                                w.WithAndSeparator();
                                                w.WithPropertyPrefix("a");
                                            }))
                                        .Or()
                                        .Where(new[]
                                        {
                                            $"{nameof(Person.Age)} = 18"
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithPropertyPrefix("b");
                                            w.WithOuterGroup();
                                        })
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' AND a.LastName = 'Fredrick' OR (b.Age = 18)";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }
                        }
                    }

                    [TestFixture]
                    public class WithOrderBy
                    {
                        [Test]
                        public void ShouldAppendOrderBy()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where(new[]
                                {
                                    $"{nameof(Person.FirstName)} = 'John'",
                                    $"{nameof(Person.LastName)} = 'Watson'"
                                }, w => { w.WithAndSeparator(); })
                                .AppendWhere(f =>
                                    f.Where(new[]
                                    {
                                        $"{nameof(Person.LastName)} = 'Fredrick'"
                                    }))
                                .OrderBy("c.Age DESC")
                                .Build();
                            // act
                            var expected =
                                "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick' ORDER BY c.Age DESC";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }

                        [TestFixture]
                        public class WithMultipleOrderBy
                        {
                            [Test]
                            public void ShouldAppendOrderBys()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where(new[]
                                    {
                                        $"{nameof(Person.FirstName)} = 'John'",
                                        $"{nameof(Person.LastName)} = 'Watson'"
                                    }, w =>
                                    {
                                        w.WithAndSeparator();
                                    })
                                    .AppendWhere(f =>
                                        f.Where(new[]
                                        {
                                            $"{nameof(Person.LastName)} = 'Fredrick'"
                                        }))
                                    .OrderBy<Person>(p => new[]
                                    {
                                        $"{nameof(p.Age)}",
                                        $"{nameof(p.FirstName)}",
                                        $"{nameof(p.LastName)}"
                                    }, o => o.WithDesc())
                                    .Build();
                                // act
                                var expected =
                                    "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick' ORDER BY c.Age DESC, c.FirstName DESC, c.LastName DESC";
                                // assert
                                Expect(sql).To.Equal(expected);
                            }
                        }
                    }
                }


                [TestFixture]
                public class GenericArrayWhere
                {
                    [TestFixture]
                    public class WithPrefix
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} = 'Ken'",
                                    $"{nameof(p.LastName)} = 'Block'"
                                }, w => { w.WithPropertyPrefix("ac"); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE ac.FirstName = 'Ken' OR ac.LastName = 'Block'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }

                        [TestFixture]
                        public class AndAnd
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.FirstName)} = 'Kevin'",
                                        $"{nameof(p.LastName)} = 'Nealon'"
                                    }, w =>
                                    {
                                        w.WithPropertyPrefix("ac");
                                        w.WithAndSeparator();
                                    })
                                    .Build();
                                // act
                                var expected =
                                    "SELECT * FROM c WHERE ac.FirstName = 'Kevin' AND ac.LastName = 'Nealon'";
                                // assert
                                Expect(sql).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class AndConditionalIf
                        {
                            [TestFixture]
                            public class WhenConditionIsTrue
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll(s => s.WithSqlVariant(Variant.MySql))
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Kevin'",
                                            $"{nameof(p.LastName)} = 'Nealon'"
                                        }, w =>
                                        {
                                            w.WithPropertyPrefix("ac");
                                            w.WithAndSeparator();
                                        })
                                        .AppendIf(() => 2 >= 1, "LIMIT 100")
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE ac.FirstName = 'Kevin' AND ac.LastName = 'Nealon' LIMIT 100;";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WhenConditionIsFalse
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll(s => s.WithSqlVariant(Variant.MySql))
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Kevin'",
                                            $"{nameof(p.LastName)} = 'Nealon'"
                                        }, w =>
                                        {
                                            w.WithPropertyPrefix("ac");
                                            w.WithAndSeparator();
                                        })
                                        .AppendIf(() => 2 >= 5, "LIMIT 100")
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE ac.FirstName = 'Kevin' AND ac.LastName = 'Nealon';";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }
                        }
                    }

                    [TestFixture]
                    public class WithoutPrefix
                    {
                        [Test]
                        public void ShouldReturnStatementWithNoPrefix()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} = 'John'",
                                    $"{nameof(p.LastName)} = 'Watson'"
                                }, w => { w.WithoutPropertyPrefix(); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE FirstName = 'John' OR LastName = 'Watson'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAndWithoutPrefix
                    {
                        [Test]
                        public void ShouldThrow()
                        {
                            // arrange
                            // act
                            // assert
                            Expect(() => new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.FirstName)} = 'John'",
                                        $"{nameof(p.LastName)} = 'Watson'"
                                    }, w =>
                                    {
                                        w.WithoutPropertyPrefix();
                                        w.WithPropertyPrefix("a");
                                    })
                                    .Build())
                                .To.Throw<InvalidStatementException>()
                                .With.Message
                                .Containing(
                                    "When building up a WHERE clause, you can not use .WithPrefix() and .WithoutPrefix() simultaneously");
                        }
                    }

                    [TestFixture]
                    public class WithOrSeparator
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} = 'John'",
                                    $"{nameof(p.LastName)} = 'Watson'"
                                }, w => { w.WithOrSeparator(); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE c.FirstName = 'John' OR c.LastName = 'Watson'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAndSeparator
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} = 'John'",
                                    $"{nameof(p.LastName)} = 'Watson'"
                                }, w => { w.WithAndSeparator(); })
                                .Build();
                            // act
                            var expected = "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }
                    }

                    [TestFixture]
                    public class WithAndOrSeparator
                    {
                        [Test]
                        public void ShouldThrow()
                        {
                            // arrange
                            // act
                            // assert
                            Expect(() => new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.FirstName)} = 'John'",
                                        $"{nameof(p.LastName)} = 'Watson'"
                                    }, w =>
                                    {
                                        w.WithAndSeparator();
                                        w.WithOrSeparator();
                                    })
                                    .Build())
                                .To.Throw<InvalidStatementException>()
                                .With.Message
                                .Containing(
                                    "When building up a WHERE clause, the options 'HasOrSeparator' and 'HasAndSeparator' can not be used in conjunction");
                        }
                    }

                    [TestFixture]
                    public class WithAppend
                    {
                        [Test]
                        public void ShouldReturnExpectedStatement()
                        {
                            // arrange
                            var sql = new SqlBuilder()
                                .SelectAll()
                                .From("c")
                                .Where<Person>(p => new[]
                                {
                                    $"{nameof(p.FirstName)} = 'John'",
                                    $"{nameof(p.LastName)} = 'Watson'"
                                }, w => { w.WithAndSeparator(); })
                                .AppendWhere(f =>
                                    f.Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.LastName)} = 'Fredrick'"
                                    }))
                                .Build();
                            // act
                            var expected =
                                "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR c.LastName = 'Fredrick'";
                            // assert
                            Expect(sql).To.Equal(expected);
                        }

                        [TestFixture]
                        public class WithNewPrefix
                        {
                            [Test]
                            public void ShouldReturnExpectedStatement()
                            {
                                // arrange
                                var sql = new SqlBuilder()
                                    .SelectAll()
                                    .From("c")
                                    .Where<Person>(p => new[]
                                    {
                                        $"{nameof(p.FirstName)} = 'John'",
                                        $"{nameof(p.LastName)} = 'Watson'"
                                    }, w => { w.WithAndSeparator(); })
                                    .AppendWhere(f =>
                                        f.Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.LastName)} = 'Fredrick'"
                                        }, w => w.WithPropertyPrefix("a")))
                                    .Build();
                                // act
                                var expected =
                                    "SELECT * FROM c WHERE c.FirstName = 'John' AND c.LastName = 'Watson' OR a.LastName = 'Fredrick'";
                                // assert
                                Expect(sql).To.Equal(expected);
                            }
                        }

                        [TestFixture]
                        public class WhereGroupings
                        {
                            [TestFixture]
                            public class WhenTwoStatementsAreGroupedWithAndOrAnd
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'John'",
                                            $"{nameof(p.LastName)} = 'Watson'"
                                        }, w =>
                                        {
                                            w.WithAndSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Or()
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Walter'",
                                            $"{nameof(p.LastName)} = 'White'"
                                        }, w =>
                                        {
                                            w.WithAndSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE (c.FirstName = 'John' AND c.LastName = 'Watson') OR (c.FirstName = 'Walter' AND c.LastName = 'White')";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WhenTwoStatementsAreGroupedWithOrAndOr
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'John'",
                                            $"{nameof(p.LastName)} = 'Watson'"
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .And()
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Walter'",
                                            $"{nameof(p.LastName)} = 'White'"
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE (c.FirstName = 'John' OR c.LastName = 'Watson') AND (c.FirstName = 'Walter' OR c.LastName = 'White')";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WhenTwoStatementsAreGroupedWithAndAndOr
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'John'",
                                            $"{nameof(p.LastName)} = 'Watson'"
                                        }, w =>
                                        {
                                            w.WithAndSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .And()
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Walter'",
                                            $"{nameof(p.LastName)} = 'White'"
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE (c.FirstName = 'John' AND c.LastName = 'Watson') AND (c.FirstName = 'Walter' OR c.LastName = 'White')";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WhenTwoStatementsAreGroupedWithOrOrAnd
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'John'",
                                            $"{nameof(p.LastName)} = 'Watson'"
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Or()
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'Walter'",
                                            $"{nameof(p.LastName)} = 'White'"
                                        }, w =>
                                        {
                                            w.WithAndSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE (c.FirstName = 'John' OR c.LastName = 'Watson') OR (c.FirstName = 'Walter' AND c.LastName = 'White')";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }

                            [TestFixture]
                            public class WithComplexNestedGrouping
                            {
                                [Test]
                                public void ShouldReturnExpectedStatement()
                                {
                                    // arrange
                                    var sql = new SqlBuilder()
                                        .SelectAll()
                                        .From("c")
                                        .Where<Person>(p => new[]
                                        {
                                            $"{nameof(p.FirstName)} = 'John'",
                                        }, w =>
                                        {
                                            w.WithOrSeparator();
                                            w.WithOuterGroup();
                                        })
                                        .And()
                                        .NestedWhere(w =>
                                            w.NestedWhere(n =>
                                                    n.Where<Person>(p => new[]
                                                                {$"{nameof(p.LastName)} = 'John'"},
                                                            o => { o.WithOuterGroup(); })
                                                        .And()
                                                        .Where<Person>(p => new[]
                                                        {
                                                            $"{nameof(p.Age)} = 18",
                                                            $"{nameof(p.Age)} = 21",
                                                            $"{nameof(p.Age)} = 25"
                                                        }, o =>
                                                        {
                                                            o.WithOrSeparator();
                                                            o.WithOuterGroup();
                                                        }))
                                                .Or()
                                                .NestedWhere(n => n
                                                    .Where<Person>(p => new[] {$"{nameof(p.FirstName)} = 'Malcom'"},
                                                        o => o.WithOuterGroup())
                                                    .And()
                                                    .Where<Person>(p => new[] {$"{nameof(p.LastName)} = 'Watson'"},
                                                        o => o.WithOuterGroup())
                                                ))
                                        .Build();
                                    // act
                                    var expected =
                                        "SELECT * FROM c WHERE (c.FirstName = 'John') AND ( ( (c.LastName = 'John') AND (c.Age = 18 OR c.Age = 21 OR c.Age = 25) ) OR ( (c.FirstName = 'Malcom') AND (c.LastName = 'Watson') ) )";
                                    // assert
                                    Expect(sql).To.Equal(expected);
                                }
                            }
                        }
                    }
                }
            }
        }

        [TestFixture]
        public class WithoutWhereClause
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .SelectAll()
                    .From("Families")
                    .Build();
                // act
                const string expected = "SELECT * FROM Families";
                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }

    [TestFixture]
    public class WithAppendAfterFrom
    {
        [Test]
        public void ShouldReturnExpectedResult()
        {
            // arrange
            var sql = new SqlBuilder()
                .Select<Person>()
                .From("people")
                .Append("JOIN t FROM j")
                .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                .Build();
            // act
            const string expected = "SELECT people.FirstName, people.LastName, people.Age FROM people JOIN t FROM j WHERE people.FirstName = 'John'";
            // assert

            Expect(sql).To.Equal(expected);
        }
    }

    [TestFixture]
    public class VariantTesting
    {
        [TestFixture]
        public class WithMySqlVariant
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Person>(o =>
                    {
                        o.WithSqlVariant(Variant.MySql);
                        o.WithPropertyCasing(Casing.SnakeCase);
                    })
                    .From("people")
                    .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                    .Build();
                // act
                var expected = "SELECT people.`first_name`, people.`last_name`, people.`age` FROM people WHERE people.`first_name` = \'John\';";
                // assert
                Expect(sql).To.Equal(expected);
            }
        }
        
        [TestFixture]
        public class WithMsSqlVariant
        {
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Person>(o => o.WithSqlVariant(Variant.MsSql))
                    .From("people")
                    .Where<Person>(p => nameof(p.FirstName), o => o.EqualsString("John"))
                    .Build();
                // act
                const string expected = "SELECT people.[FirstName], people.[LastName], people.[Age] FROM people WHERE people.[FirstName] = \'John\';";
                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }
}

public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
}

public class Car
{
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public int Age { get; set; }
}

public class Manager
{
    public int RoleId { get; set; }
}