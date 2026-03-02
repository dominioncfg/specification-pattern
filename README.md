# Specification Pattern Example

A .NET 10 solution demonstrating the **Specification Pattern** applied to both in-memory domain collections and EF Core database queries.

## Projects

| Project | Description |
|---|---|
| `SpecificationExample.Domain` | Core domain entities, specification abstractions, and repository interfaces |
| `SpecificationExample.Host` | Console host that exercises both EF Core and in-memory specifications |
| `SpecificationExample.UnitTests` | xUnit unit tests covering filtering, ordering, paging, and logical operators |
| `SpecificationExample.AcceptanceTests` | Reqnroll (BDD) acceptance tests for in-memory specifications |

## Key Concepts

### Domain Specifications (In-Memory)

`DomainSpecification<T>` supports filtering, ordering, and paging over in-memory collections.

```csharp
public class BlogAccountByNameSpecification : DomainSpecification<BlogAccount>
{
    public BlogAccountByNameSpecification(string name)
    {
        Rule(query => name == query.Name);
    }
}

// Usage
var spec = new BlogAccountByNameSpecification("First Blog Account");
var results = blogAccounts.Filter(spec);
```

Specifications can be combined using logical operators:

```csharp
var combined = specA.And(specB);
var either   = specA.Or(specB);
var negated  = spec.Not();
```

### Query Specifications (EF Core)

`QuerySpecification<T>` translates specifications into EF Core queries with support for filtering, eager loading, ordering, and paging.

```csharp
var filter = new BlogAccountAggregateQuerySpecification();
var blogAccounts = await blogRepository.Filter(filter, cancellationToken);
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run the host

```bash
dotnet run --project SpecificationExample.Host
```

### Run the tests

```bash
dotnet test
```

## Solution Structure

```
SpecificationExample/
├── SpecificationExample.Domain/
│   ├── Blogs/
│   │   ├── Entities/          # BlogAccount, Blog, Post, Autor, …
│   │   ├── Repositories/      # IBlogAccountRepository
│   │   └── Specifications/    # Domain & query specifications
│   └── Common/
│       ├── Specification/
│       │   ├── Domain/        # DomainSpecification<T> + AND/OR/NOT operators
│       │   └── Query/         # QuerySpecification<T> + include/orderby builders
│       └── Repositories/      # IRepository<T>
├── SpecificationExample.Host/
│   ├── Infra/                 # EF Core DbContext, repository implementation
│   └── Program.cs
├── SpecificationExample.UnitTests/
└── SpecificationExample.AcceptanceTests/
    └── Features/              # Reqnroll .feature files
```
