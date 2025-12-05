using NExpect;
using NUnit.Framework;
using ScopeFunction.GenericSqlBuilder.Attributes;
using ScopeFunction.GenericSqlBuilder.Enums;
using static NExpect.Expectations;

namespace ScopeFunction.GenericSqlBuilder.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class ColumnNameAttributeTests
{
    [TestFixture]
    public class SelectStatement
    {
        [TestFixture]
        public class WithColumnNameAttribute
        {
            [Test]
            public void ShouldUseColumnNameAttributeValueInsteadOfPropertyName()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Employee>()
                    .From("employees")
                    .Build();

                // act
                const string expected = "SELECT employees.Id, employees.first_name, employees.last_name, employees.DepartmentId FROM employees";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithSnakeCaseCasing()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Employee>(o => o.WithPropertyCasing(Casing.SnakeCase))
                    .From("employees")
                    .Build();

                // act
                // first_name and last_name should use [ColumnName] value
                // Id and DepartmentId should be converted to snake_case
                const string expected = "SELECT employees.id, employees.first_name, employees.last_name, employees.department_id FROM employees";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithPrefix()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Select<Employee>(o =>
                    {
                        o.WithPropertyPrefix("e");
                        o.WithPropertyCasing(Casing.SnakeCase);
                    })
                    .From("employees e")
                    .Build();

                // act
                const string expected = "SELECT e.id, e.first_name, e.last_name, e.department_id FROM employees e";

                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }

    [TestFixture]
    public class InsertStatement
    {
        [TestFixture]
        public class WithColumnNameAttribute
        {
            [Test]
            public void ShouldUseColumnNameAttributeValueInsteadOfPropertyName()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Insert<Employee>()
                    .Into("employees")
                    .Build();

                // act
                const string expected = "INSERT INTO employees (Id, first_name, last_name, DepartmentId) VALUES (@Id, @FirstName, @LastName, @DepartmentId)";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithSnakeCaseCasing()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Insert<Employee>(o => o.WithPropertyCasing(Casing.SnakeCase))
                    .Into("employees")
                    .Build();

                // act
                // first_name and last_name should use [ColumnName] value
                // Id and DepartmentId should be converted to snake_case
                const string expected = "INSERT INTO employees (id, first_name, last_name, department_id) VALUES (@Id, @FirstName, @LastName, @DepartmentId)";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithUpdateOnDuplicateKey()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Insert<Employee>(o =>
                    {
                        o.WithPropertyCasing(Casing.SnakeCase);
                        o.WithUpdateOnDuplicateKey(e => e.FirstName, e => e.LastName);
                    })
                    .Into("employees")
                    .Build();

                // act
                const string expected = "INSERT INTO employees (id, first_name, last_name, department_id) VALUES (@Id, @FirstName, @LastName, @DepartmentId) ON DUPLICATE KEY UPDATE first_name = @FirstName, last_name = @LastName";

                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }

    [TestFixture]
    public class UpdateStatement
    {
        [TestFixture]
        public class WithColumnNameAttribute
        {
            [Test]
            public void ShouldUseColumnNameAttributeValueInsteadOfPropertyName()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update<Employee>("employees")
                    .Set()
                    .Where("id = 1")
                    .Build();

                // act
                const string expected = "UPDATE employees SET Id = @Id, first_name = @FirstName, last_name = @LastName, DepartmentId = @DepartmentId WHERE id = 1";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithSnakeCaseCasing()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update<Employee>("employees", o => o.WithPropertyCasing(Casing.SnakeCase))
                    .Set()
                    .Where("id = 1")
                    .Build();

                // act
                // first_name and last_name should use [ColumnName] value
                // Id and DepartmentId should be converted to snake_case
                const string expected = "UPDATE employees SET id = @Id, first_name = @FirstName, last_name = @LastName, department_id = @DepartmentId WHERE id = 1";

                // assert
                Expect(sql).To.Equal(expected);
            }

            [Test]
            public void ShouldUseColumnNameAttributeValueWithWithoutProperty()
            {
                // arrange
                var sql = new SqlBuilder()
                    .Update<Employee>("employees", o =>
                    {
                        o.WithPropertyCasing(Casing.SnakeCase);
                        o.WithoutProperties(e => e.Id);
                    })
                    .Set()
                    .Where("id = 1")
                    .Build();

                // act
                const string expected = "UPDATE employees SET first_name = @FirstName, last_name = @LastName, department_id = @DepartmentId WHERE id = 1";

                // assert
                Expect(sql).To.Equal(expected);
            }
        }
    }
}

public class Employee
{
    public int Id { get; set; }

    [ColumnName("first_name")]
    public string? FirstName { get; set; }

    [ColumnName("last_name")]
    public string? LastName { get; set; }

    public int DepartmentId { get; set; }
}
