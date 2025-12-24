using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenOrderingBySingleSpecification
{
    [Fact]
    public void Orders_By_Name_Ascending()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 30 },
            };

        var specification = new TestEntityOrderByNameAscendingSpecification();

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(3);
        orderedEntities[0].Name.Should().Be("Alpha");
        orderedEntities[1].Name.Should().Be("Bravo");
        orderedEntities[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Orders_By_Name_Descending()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 30 },
            };

        var specification = new TestEntityOrderByNameDescendingSpecification();

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(3);
        orderedEntities[0].Name.Should().Be("Charlie");
        orderedEntities[1].Name.Should().Be("Bravo");
        orderedEntities[2].Name.Should().Be("Alpha");
    }

    [Fact]
    public void Orders_By_Multiple_Properties_Using_ThenBy()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            };

        var specification = new TestEntityOrderByValueThenByNameSpecification();

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(4);
        orderedEntities[0].Should().Be(entities[2]);
        orderedEntities[1].Should().Be(entities[3]);
        orderedEntities[2].Should().Be(entities[1]);
        orderedEntities[3].Should().Be(entities[0]);
    }

    [Fact]
    public void Orders_By_Multiple_Properties_With_Mixed_Sort_Directions()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
            };

        var specification = new TestEntityOrderByValueDescendingThenByNameAscendingSpecification();

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(4);
        orderedEntities[0].Should().Be(entities[1]); // Alpha, 20
        orderedEntities[1].Should().Be(entities[0]); // Bravo, 20
        orderedEntities[2].Should().Be(entities[2]); // Alpha, 10
        orderedEntities[3].Should().Be(entities[3]); // Bravo, 10
    }

    [Fact]
    public void Orders_With_Filter_Applied()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 20 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Delta", Value = 20 },
            };

        var specification = new TestEntityByValueWithOrderByNameSpecification(20);

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(3);
        orderedEntities[0].Name.Should().Be("Alpha");
        orderedEntities[1].Name.Should().Be("Charlie");
        orderedEntities[2].Name.Should().Be("Delta");
    }

    [Fact]
    public void Returns_All_Entities_When_Only_Ordering_Is_Applied()
    {
        var entities = new List<TestEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Charlie", Value = 30 },
                new() { Id = Guid.NewGuid(), Name = "Alpha", Value = 10 },
                new() { Id = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            };

        var specification = new TestEntityOrderByNameAscendingSpecification();

        var orderedEntities = entities.Filter(specification).ToList();

        orderedEntities.Should().HaveCount(3);
    }

    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class TestEntityOrderByNameAscendingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByNameAscendingSpecification()
        {
            OrderBy(e => e.Name);
        }
    }

    private class TestEntityOrderByNameDescendingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByNameDescendingSpecification()
        {
            OrderByDescending(e => e.Name);
        }
    }

    private class TestEntityOrderByValueAscendingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByValueAscendingSpecification()
        {
            OrderBy(e => e.Value);
        }
    }

    private class TestEntityOrderByValueDescendingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByValueDescendingSpecification()
        {
            OrderByDescending(e => e.Value);
        }
    }

    private class TestEntityOrderByValueThenByNameSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByValueThenByNameSpecification()
        {
            OrderBy(e => e.Value);
            OrderBy(e => e.Name);
        }
    }

    private class TestEntityOrderByValueDescendingThenByNameAscendingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByValueDescendingThenByNameAscendingSpecification()
        {
            OrderByDescending(e => e.Value);
            OrderBy(e => e.Name);
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

    private class TestEntityByValueSpecification : Specification<TestEntity>
    {
        public TestEntityByValueSpecification(int value)
        {
            Rule(e => e.Value == value);
        }
    }
}
