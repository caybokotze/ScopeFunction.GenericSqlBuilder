using NUnit.Framework;

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
            [Test]
            public void ShouldReturnExpectedStatement()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update("");
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
                
                // act
                // assert
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
                
                // act
                // assert
            }
        }
    }
}