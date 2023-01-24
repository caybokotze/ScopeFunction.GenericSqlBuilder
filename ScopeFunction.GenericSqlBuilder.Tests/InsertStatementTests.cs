using NUnit.Framework;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
public class InsertStatementTests
{
    [TestFixture]
    public class WithGenericBuilder
    {
        [TestFixture]
        public class WithAnnotations
        {
            [TestFixture]
            public class WithOptions
            {
                
            }

            [TestFixture]
            public class WithoutOptions
            {
                
            }
        }

        [TestFixture]
        public class WithoutAnnotations
        {
            [TestFixture]
            public class WithOptions
            {
            
            }

            [TestFixture]
            public class WithoutOptions
            {
            
            }
        }
    }

    [TestFixture]
    public class WithoutGenericBuilder
    {
        [TestFixture]
        public class WithOptions
        {
            
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
                    .Into("c")
                    .Build();
                // act
                var expected = "INSERT INTO people (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age)";
                // assert
            }
        }
    }
}