using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenFilteringBySingleSpecification
{
    [Fact]
    public void Single_Item_Is_Returned()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Entity1", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Entity2", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Entity3", Value = 30 },
        };

        var specification = new TestEntityByNameSpecification("Entity2");

        var filteredEntities = entities.Filter(specification).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[1]);
    }

    [Fact]
    public void No_Item_Is_Returned()
    {
        TestEntity testEntity = new() { Id = Guid.NewGuid(), Name = "Entity3", Value = 30 };
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Entity1", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Entity2", Value = 20 },
            testEntity,
        };

        var specification = new TestEntityByNameSpecification("Entity4");

        var filteredEntities = entities.Filter(specification).ToList();

        filteredEntities.Should().BeEmpty();
    }

    [Fact]
    public void Multiple_Items_Are_Returned()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Entity1", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Entity2", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Entity3", Value = 20 },
        };
        var specification = new TestEntityByValueSpecification(20);
        var filteredEntities = entities.Filter(specification).ToList();
        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Returns_All_Entities_When_Specification_Has_No_Rules()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Entity1", Value = 10 },
            new() { Id = Guid.NewGuid(), Name = "Entity2", Value = 20 },
            new() { Id = Guid.NewGuid(), Name = "Entity3", Value = 20 },
        };
        var specification = new ReturnsAllTestEntitiesSpecification();
        var filteredEntities = entities.Filter(specification).ToList();
        filteredEntities.Should().HaveCount(3);
    }


    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class ReturnsAllTestEntitiesSpecification : DomainSpecification<TestEntity>
    {
        public ReturnsAllTestEntitiesSpecification()
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
}
