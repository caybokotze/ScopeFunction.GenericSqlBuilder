# Query Builders Overview
Good day and welcome to the thing...

### Overview
I wrote this package to make writing sql queries that need to be maintainable a little bit more pleasant in certain use cases. The most common use case for this might be when using **dapper** or when writing raw NO SQL queries for **CosmosDB** or any other database where this is relevant.

**What is the point?** 

Mainly to have a C# based query syntax with **strongly typed property names**. Therefore, if a property name changes in your code base, all your queries are still valid.
Another perk is that should you need to migrate your queries to another platform, you can just change a few options and "Bobs your uncle!" -- it should work.

### Supported platforms

- [X] CosmosDb
- [ ] MySql / MariaDb
- [ ] SqlServer
- [ ] PostgreSql



### Package behaviour 
- When no prefix options are provided the table name will be appended to all SELECT, WHERE and ORDER BY clauses.
- If no casing options are provided the properties will remain with the default casing of the nameof(T) string output.  

## Usage Examples

### Complete SELECT with WHERE
#### Fluent C# builder
```csharp
var sql = 
    new SqlBuilder()
        .Select(new []
        {
            nameof(Person.FirstName),
            nameof(Person.LastName),
            nameof(Person.Age)
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
        .Where(nameof(Person.FirstName), o => o.EqualsString("John"))
        .And()
        .Where<Person>(p => nameof(p.LastName), w => w.Like("Williams"))
        .Build();
```
#### Generates
```genericsql
SELECT 
    c.FirstName, 
    c.LastName, 
    c.Age 
FROM c WHERE w.Age = 18 
        OR w.Age = 20 
        AND FirstName like %John% 
        OR FirstName = 'John' 
        AND LastName LIKE 'Williams'
```

### SELECT without WHERE
#### Fluent C# builder
#### Generates

### SELECT with WHERE AND ORDER BY
#### Fluent C# builder
#### Generates

### SELECT with WHERE (with explicit select prefix)
#### Fluent C# builder
#### Generates

### SELECT with JOIN AND WHERE (with explicit SELECT and WHERE prefix)
#### Fluent C# builder
#### Generates


## SELECT builder variants
### With options

### Without options

## WHERE builder variants
### With options

### Without options

## Use static builder

```csharp
using static QueryBuilders;

Query(q => q.SelectAll()
    .From("people")
    .Where<Person>(nameof(w.FirstName), w => w.Like("John")));
```

## Set default options for all queries
 If you would like to specify options that should be applied to all your queries, simply use the ConfigurationBuilder.



## Contributions and bug fixes
If you have any suggestions, bug fixes or features you would like to add to improve this code, fell free to tag me in a PR and I will have a look as soon as possible.

Cheerio! C.K.
