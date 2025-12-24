using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenUsingAndSpecification
{
    [Fact]
    public void Combines_Two_Filters_With_And_Logic()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 30 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var valueSpec = new TestEntityByValueSpecification(20);
        var andSpec = nameSpec.And(valueSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[0]);
    }

    [Fact]
    public void Returns_Empty_When_No_Entities_Match_Both_Conditions()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var valueSpec = new TestEntityByValueSpecification(20);
        var andSpec = nameSpec.And(valueSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().BeEmpty();
    }

    [Fact]
    public void Returns_Multiple_Entities_Matching_Both_Conditions()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 20 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var rangeSpec = new TestEntityByValueGreaterThanSpecification(15);
        var andSpec = valueSpec.And(rangeSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(3);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[3]);
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
        var andSpec = nameSpec.And(emptySpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

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
        var andSpec = emptySpec.And(valueSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Throws_When_Both_Specifications_Are_Null()
    {
        var act = () => new AndSpecification<TestEntity>(null!, null!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Both specifications cannot be null");
    }

    [Fact]
    public void Combines_Filters_And_Preserves_OrderBy_From_Both_Specifications()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
        };

        var valueSpec = new TestEntityByValueWithOrderByNameSpecification(20);
        var rangeSpec = new TestEntityByValueGreaterThanSpecification(15);
        var andSpec = valueSpec.And(rangeSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(3);
        filteredEntities[0].Name.Should().Be("Alpha");
        filteredEntities[1].Name.Should().Be("Bravo");
        filteredEntities[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Combines_Filters_And_Preserves_Paging_From_Left_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
        };

        var valueSpecWithPaging = new TestEntityByValueWithPagingSpecification(20, 1, 1);
        var rangeSpec = new TestEntityByValueGreaterThanSpecification(15);
        var andSpec = valueSpecWithPaging.And(rangeSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[1]);
    }

    [Fact]
    public void Combines_Filters_And_Uses_Right_Paging_When_Left_Has_None()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var rangeSpecWithPaging = new TestEntityByValueGreaterThanWithPagingSpecification(15, 0, 2);
        var andSpec = valueSpec.And(rangeSpecWithPaging);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Should().Be(entities[0]);
        filteredEntities[1].Should().Be(entities[1]);
    }

    [Fact]
    public void Chains_Multiple_And_Specifications()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 25 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 15 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 25 },
        };

        var valueSpec = new TestEntityByValueSpecification(25);
        var rangeSpec = new TestEntityByValueGreaterThanSpecification(20);
        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var andSpec = valueSpec.And(rangeSpec).And(nameSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[0]);
    }

    [Fact]
    public void Combines_Complex_Filters_With_OrderBy_And_Paging()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 25 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 25 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 25 },
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 15 },
            new() { Id = Guid.NewGuid(), Name = "Echo", Value = 25 },
        };

        var valueSpecWithOrderBy = new TestEntityByValueWithOrderByNameSpecification(25);
        var rangeSpecWithPaging = new TestEntityByValueGreaterThanWithPagingSpecification(20, 1, 2);
        var andSpec = valueSpecWithOrderBy.And(rangeSpecWithPaging);

        var filteredEntities = entities.Filter(andSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Bravo");
        filteredEntities[1].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Applies_Filter_Before_OrderBy_And_Paging()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Delta", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Echo", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var orderAndPageSpec = new TestEntityOrderByNameWithPagingSpecification(0, 2);
        var andSpec = valueSpec.And(orderAndPageSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        // Should filter for Value=20, then order by name, then take first 2
        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Alpha");
        filteredEntities[1].Name.Should().Be("Bravo");
    }

    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class TestEntityEmptySpecification : Specification<TestEntity>
    {
        public TestEntityEmptySpecification()
        {
        }
    }

    private class TestEntityByNameSpecification : Specification<TestEntity>
    {
        public TestEntityByNameSpecification(string name)
        {
            Rule(e => e.Name == name);
        }
    }

    private class TestEntityByValueSpecification : Specification<TestEntity>
    {
        public TestEntityByValueSpecification(int value)
        {
            Rule(e => e.Value == value);
        }
    }

    private class TestEntityByValueGreaterThanSpecification : Specification<TestEntity>
    {
        public TestEntityByValueGreaterThanSpecification(int value)
        {
            Rule(e => e.Value > value);
        }
    }

    private class TestEntityByValueWithOrderByNameSpecification : Specification<TestEntity>
    {
        public TestEntityByValueWithOrderByNameSpecification(int value)
        {
            Rule(e => e.Value == value);
            OrderBy(e => e.Name);
        }
    }

    private class TestEntityByValueWithPagingSpecification : Specification<TestEntity>
    {
        public TestEntityByValueWithPagingSpecification(int value, int skip, int take)
        {
            Rule(e => e.Value == value);
            Paginate(skip, take);
        }
    }

    private class TestEntityByValueGreaterThanWithPagingSpecification : Specification<TestEntity>
    {
        public TestEntityByValueGreaterThanWithPagingSpecification(int value, int skip, int take)
        {
            Rule(e => e.Value > value);
            Paginate(skip, take);
        }
    }

    private class TestEntityOrderByNameWithPagingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByNameWithPagingSpecification(int skip, int take)
        {
            OrderBy(e => e.Name);
            Paginate(skip, take);
        }
    }
}