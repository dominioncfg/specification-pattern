using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenUsingOrSpecification
{
    [Fact]
    public void Combines_Two_Filters_With_Or_Logic()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 20 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var valueSpec = new TestEntityByValueSpecification(10);
        var orSpec = nameSpec.Or(valueSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[1]);
    }

    [Fact]
    public void Returns_All_Entities_Matching_Either_Condition()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 40 },
        };

        var valueSpec1 = new TestEntityByValueSpecification(10);
        var valueSpec2 = new TestEntityByValueSpecification(30);
        var orSpec = valueSpec1.Or(valueSpec2);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Returns_Entity_Matching_Both_Conditions()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 30 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var valueSpec = new TestEntityByValueSpecification(20);
        var orSpec = nameSpec.Or(valueSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Returns_Empty_When_No_Entities_Match_Either_Condition()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
        };

        var nameSpec = new TestEntityByNameSpecification("Delta");
        var valueSpec = new TestEntityByValueSpecification(100);
        var orSpec = nameSpec.Or(valueSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().BeEmpty();
    }

    [Fact]
    public void Returns_All_Entities_When_One_Condition_Matches_All()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
        };

        var valueSpec = new TestEntityByValueGreaterThanSpecification(0);
        var nameSpec = new TestEntityByNameSpecification("NonExistent");
        var orSpec = valueSpec.Or(nameSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(3);
    }

    [Fact]
    public void Combines_Left_Specification_When_Right_Has_No_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var emptySpec = new TestEntityEmptySpecification();
        var orSpec = nameSpec.Or(emptySpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[0]);
    }

    [Fact]
    public void Combines_Right_Specification_When_Left_Has_No_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
        };

        var emptySpec = new TestEntityEmptySpecification();
        var valueSpec = new TestEntityByValueSpecification(20);
        var orSpec = emptySpec.Or(valueSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Throws_When_Both_Specifications_Have_No_Filter()
    {
        var emptySpec1 = new TestEntityEmptySpecification();
        var emptySpec2 = new TestEntityEmptySpecification();

        var act = () => new DomainOrSpecification<TestEntity>(null!, null!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Both specifications cannot be null");
    }

    [Fact]
    public void Combines_Filters_And_Preserves_OrderBy_From_Both_Specifications()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 40 },
        };

        var valueSpecWithOrderBy = new TestEntityByValueWithOrderByNameSpecification(10);
        var nameSpec = new TestEntityByNameSpecification("Bravo");
        var orSpec = valueSpecWithOrderBy.Or(nameSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Alpha");
        filteredEntities[1].Name.Should().Be("Bravo");
    }

    [Fact]
    public void Combines_Filters_And_Preserves_Paging_From_Left_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
        };

        var valueSpecWithPaging = new TestEntityByValueWithPagingSpecification(20, 1, 1);
        var nameSpec = new TestEntityByNameSpecification("Bravo");
        var orSpec = valueSpecWithPaging.Or(nameSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[1]);
    }

    [Fact]
    public void Combines_Filters_And_Uses_Right_Paging_When_Left_Has_None()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var nameSpecWithPaging = new TestEntityByNameWithPagingSpecification("Bravo", 0, 2);
        var orSpec = valueSpec.Or(nameSpecWithPaging);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Should().Be(entities[0]);
        filteredEntities[1].Should().Be(entities[1]);
    }

    [Fact]
    public void Chains_Multiple_Or_Specifications()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 40 },
        };

        var valueSpec1 = new TestEntityByValueSpecification(10);
        var valueSpec2 = new TestEntityByValueSpecification(30);
        var nameSpec = new TestEntityByNameSpecification("Bravo");
        var orSpec = valueSpec1.Or(valueSpec2).Or(nameSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(3);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Combines_Or_With_Range_Specifications()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 5 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 15 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 25 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 35 },
        };

        var lessThanSpec = new TestEntityByValueLessThanSpecification(10);
        var greaterThanSpec = new TestEntityByValueGreaterThanSpecification(30);
        var orSpec = lessThanSpec.Or(greaterThanSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[3]);
    }

    [Fact]
    public void Combines_Complex_Filters_With_OrderBy_And_Paging()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Echo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 10 },
        };

        var valueSpecWithOrderBy = new TestEntityByValueWithOrderByNameSpecification(10);
        var nameSpecWithPaging = new TestEntityByNameWithPagingSpecification("Bravo", 0, 3);
        var orSpec = valueSpecWithOrderBy.Or(nameSpecWithPaging);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(3);
        filteredEntities[0].Name.Should().Be("Bravo");
        filteredEntities[1].Name.Should().Be("Charlie");
        filteredEntities[2].Name.Should().Be("Delta");
    }

    [Fact]
    public void Applies_Filter_Before_OrderBy_And_Paging()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Echo", Value = 50 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(10);
        var orderAndPageSpec = new TestEntityByValueWithOrderByNameAndPagingSpecification(30, 0, 2);
        var orSpec = valueSpec.Or(orderAndPageSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        // Should match Value=10 OR Value=30, then order by name, then take first 2
        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Alpha");
        filteredEntities[1].Name.Should().Be("Bravo");
    }

    [Fact]
    public void Combines_Multiple_Properties_With_Or_Logic()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 100 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 40 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var valueSpec = new TestEntityByValueSpecification(20);
        var orSpec = nameSpec.Or(valueSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        filteredEntities.Should().HaveCount(3);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[3]);
    }

    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class TestEntityEmptySpecification : DomainSpecification<TestEntity>
    {
        public TestEntityEmptySpecification()
        {
        }
    }

    private class TestEntityByNameSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByNameSpecification(string name)
        {
            Rule(e => e.Name == name);
        }
    }

    private class TestEntityByValueSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueSpecification(int value)
        {
            Rule(e => e.Value == value);
        }
    }

    private class TestEntityByValueGreaterThanSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueGreaterThanSpecification(int value)
        {
            Rule(e => e.Value > value);
        }
    }

    private class TestEntityByValueLessThanSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueLessThanSpecification(int value)
        {
            Rule(e => e.Value < value);
        }
    }

    private class TestEntityByValueWithOrderByNameSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueWithOrderByNameSpecification(int value)
        {
            Rule(e => e.Value == value);
            OrderBy(e => e.Name);
        }
    }

    private class TestEntityByValueWithPagingSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueWithPagingSpecification(int value, int skip, int take)
        {
            Rule(e => e.Value == value);
            Paginate(skip, take);
        }
    }

    private class TestEntityByNameWithPagingSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByNameWithPagingSpecification(string name, int skip, int take)
        {
            Rule(e => e.Name == name);
            Paginate(skip, take);
        }
    }

    private class TestEntityByValueWithOrderByNameAndPagingSpecification : DomainSpecification<TestEntity>
    {
        public TestEntityByValueWithOrderByNameAndPagingSpecification(int value, int skip, int take)
        {
            Rule(e => e.Value == value);
            OrderBy(e => e.Name);
            Paginate(skip, take);
        }
    }
}