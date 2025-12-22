using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenUsingNotSpecification
{
    [Fact]
    public void Negates_Simple_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 20 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
            new() { Id = 4, Name = "Delta", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var notSpec = valueSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[3]);
    }

    [Fact]
    public void Negates_Name_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 20 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Alpha", Value = 30 },
        };

        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var notSpec = nameSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[1]);
    }

    [Fact]
    public void Negates_Greater_Than_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 30 },
            new() { Id = 4, Name = "Delta", Value = 40 },
        };

        var greaterThanSpec = new TestEntityByValueGreaterThanSpecification(20);
        var notSpec = greaterThanSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[1]);
    }

    [Fact]
    public void Negates_Less_Than_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 30 },
        };

        var lessThanSpec = new TestEntityByValueLessThanSpecification(20);
        var notSpec = lessThanSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Returns_Empty_When_All_Entities_Match_Original_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 20 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var notSpec = valueSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().BeEmpty();
    }

    [Fact]
    public void Returns_All_Entities_When_None_Match_Original_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(100);
        var notSpec = valueSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(3);
    }

    [Fact]
    public void Throws_When_Specification_Is_Null()
    {
        var act = () => new NotSpecification<TestEntity>(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Throws_When_Specification_Has_No_Filter()
    {
        var emptySpec = new TestEntityEmptySpecification();

        var act = () => emptySpec.Not();

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Preserves_OrderBy_From_Original_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Charlie", Value = 10 },
            new() { Id = 2, Name = "Alpha", Value = 10 },
            new() { Id = 3, Name = "Bravo", Value = 20 },
        };

        var valueSpecWithOrderBy = new TestEntityByValueWithOrderByNameSpecification(20);
        var notSpec = valueSpecWithOrderBy.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Alpha");
        filteredEntities[1].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Preserves_Paging_From_Original_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
            new() { Id = 4, Name = "Delta", Value = 10 },
        };

        var valueSpecWithPaging = new TestEntityByValueWithPagingSpecification(20, 1, 2);
        var notSpec = valueSpecWithPaging.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Should negate Value=20 (keeping 10s), then skip 1, take 2
        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Should().Be(entities[1]);
        filteredEntities[1].Should().Be(entities[3]);
    }

    [Fact]
    public void Preserves_OrderBy_And_Paging_From_Original_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Delta", Value = 10 },
            new() { Id = 2, Name = "Charlie", Value = 10 },
            new() { Id = 3, Name = "Bravo", Value = 20 },
            new() { Id = 4, Name = "Alpha", Value = 10 },
        };

        var valueSpecWithOrderByAndPaging = new TestEntityByValueWithOrderByNameAndPagingSpecification(20, 1, 2);
        var notSpec = valueSpecWithOrderByAndPaging.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Should negate Value=20, order by name, skip 1, take 2
        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Charlie");
        filteredEntities[1].Name.Should().Be("Delta");
    }

    [Fact]
    public void Double_Negation_Returns_Original_Results()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 20 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var notNotSpec = valueSpec.Not().Not();

        var filteredEntities = entities.Filter(notNotSpec).ToList();

        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
    }

    [Fact]
    public void Combines_Not_With_And_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 20 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
            new() { Id = 4, Name = "Delta", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var greaterThanSpec = new TestEntityByValueGreaterThanSpecification(15);
        var notValueSpec = valueSpec.Not();
        var andSpec = notValueSpec.And(greaterThanSpec);

        var filteredEntities = entities.Filter(andSpec).ToList();

        // Not Value=20 AND Value>15 = Value>15 but not 20
        filteredEntities.Should().HaveCount(1);
        filteredEntities[0].Should().Be(entities[3]);
    }

    [Fact]
    public void Combines_Not_With_Or_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 30 },
            new() { Id = 4, Name = "Delta", Value = 40 },
        };

        var valueSpec = new TestEntityByValueSpecification(20);
        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var notValueSpec = valueSpec.Not();
        var orSpec = notValueSpec.Or(nameSpec);

        var filteredEntities = entities.Filter(orSpec).ToList();

        // Not Value=20 OR Name=Alpha = all except Bravo
        filteredEntities.Should().HaveCount(3);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[2]);
        filteredEntities.Should().Contain(entities[3]);
    }

    [Fact]
    public void Negates_Complex_And_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 25 },
            new() { Id = 2, Name = "Bravo", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 20 },
            new() { Id = 4, Name = "Delta", Value = 30 },
        };

        var valueSpec = new TestEntityByValueSpecification(25);
        var nameSpec = new TestEntityByNameSpecification("Alpha");
        var andSpec = valueSpec.And(nameSpec);
        var notSpec = andSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Not (Value=25 AND Name=Alpha) = everything except Alpha with value 25
        filteredEntities.Should().HaveCount(3);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[2]);
        filteredEntities.Should().Contain(entities[3]);
    }

    [Fact]
    public void Negates_Complex_Or_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 10 },
            new() { Id = 2, Name = "Bravo", Value = 20 },
            new() { Id = 3, Name = "Charlie", Value = 30 },
            new() { Id = 4, Name = "Delta", Value = 40 },
        };

        var valueSpec1 = new TestEntityByValueSpecification(10);
        var valueSpec2 = new TestEntityByValueSpecification(30);
        var orSpec = valueSpec1.Or(valueSpec2);
        var notSpec = orSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Not (Value=10 OR Value=30) = Bravo and Delta
        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[1]);
        filteredEntities.Should().Contain(entities[3]);
    }

    [Fact]
    public void Applies_Not_Filter_Before_OrderBy_And_Paging()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Echo", Value = 20 },
            new() { Id = 2, Name = "Delta", Value = 10 },
            new() { Id = 3, Name = "Charlie", Value = 10 },
            new() { Id = 4, Name = "Bravo", Value = 10 },
            new() { Id = 5, Name = "Alpha", Value = 20 },
        };

        var valueSpecWithOrderByAndPaging = new TestEntityByValueWithOrderByNameAndPagingSpecification(20, 1, 2);
        var notSpec = valueSpecWithOrderByAndPaging.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Not Value=20, order by name, skip 1, take 2
        filteredEntities.Should().HaveCount(2);
        filteredEntities[0].Name.Should().Be("Charlie");
        filteredEntities[1].Name.Should().Be("Delta");
    }

    [Fact]
    public void Negates_Range_Specification()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Alpha", Value = 5 },
            new() { Id = 2, Name = "Bravo", Value = 15 },
            new() { Id = 3, Name = "Charlie", Value = 25 },
            new() { Id = 4, Name = "Delta", Value = 35 },
        };

        var rangeSpec = new TestEntityByValueRangeSpecification(10, 30);
        var notSpec = rangeSpec.Not();

        var filteredEntities = entities.Filter(notSpec).ToList();

        // Not (Value >= 10 AND Value <= 30) = values outside range
        filteredEntities.Should().HaveCount(2);
        filteredEntities.Should().Contain(entities[0]);
        filteredEntities.Should().Contain(entities[3]);
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

    private class TestEntityByValueLessThanSpecification : Specification<TestEntity>
    {
        public TestEntityByValueLessThanSpecification(int value)
        {
            Rule(e => e.Value < value);
        }
    }

    private class TestEntityByValueRangeSpecification : Specification<TestEntity>
    {
        public TestEntityByValueRangeSpecification(int min, int max)
        {
            Rule(e => e.Value >= min && e.Value <= max);
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

    private class TestEntityByValueWithOrderByNameAndPagingSpecification : Specification<TestEntity>
    {
        public TestEntityByValueWithOrderByNameAndPagingSpecification(int value, int skip, int take)
        {
            Rule(e => e.Value == value);
            OrderBy(e => e.Name);
            Paginate(skip, take);
        }
    }
}