using FluentAssertions;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests.Common.Specifications;

public class WhenPagingBySingleSpecification
{
    [Fact]
    public void Applies_Skip_Only()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
            new() { Id = 4, Name = "Fourth", Value = 40 },
            new() { Id = 5, Name = "Fifth", Value = 50 },
        };

        var specification = new TestEntityPaginateSkipOnlySpecification(2);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(3);
        pagedEntities[0].Id.Should().Be(3);
        pagedEntities[1].Id.Should().Be(4);
        pagedEntities[2].Id.Should().Be(5);
    }

    [Fact]
    public void Applies_Take_Only()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
            new() { Id = 4, Name = "Fourth", Value = 40 },
            new() { Id = 5, Name = "Fifth", Value = 50 },
        };

        var specification = new TestEntityPaginateTakeOnlySpecification(3);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(3);
        pagedEntities[0].Id.Should().Be(1);
        pagedEntities[1].Id.Should().Be(2);
        pagedEntities[2].Id.Should().Be(3);
    }

    [Fact]
    public void Applies_Skip_And_Take()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
            new() { Id = 4, Name = "Fourth", Value = 40 },
            new() { Id = 5, Name = "Fifth", Value = 50 },
        };

        var specification = new TestEntityPaginateSpecification(1, 2);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(2);
        pagedEntities[0].Id.Should().Be(2);
        pagedEntities[1].Id.Should().Be(3);
    }

    [Fact]
    public void Skip_Zero_Returns_All_Elements()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
        };

        var specification = new TestEntityPaginateSkipOnlySpecification(0);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(3);
    }

    [Fact]
    public void Skip_Greater_Than_Collection_Size_Returns_Empty()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
        };

        var specification = new TestEntityPaginateSkipOnlySpecification(10);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().BeEmpty();
    }

    [Fact]
    public void Take_Greater_Than_Collection_Size_Returns_All_Available()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
        };

        var specification = new TestEntityPaginateTakeOnlySpecification(100);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(2);
    }

    [Fact]
    public void Applies_Paging_With_Filter()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 20 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 10 },
            new() { Id = 4, Name = "Fourth", Value = 20 },
            new() { Id = 5, Name = "Fifth", Value = 20 },
        };

        var specification = new TestEntityByValueWithPagingSpecification(20, 1, 2);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(2);
        pagedEntities[0].Id.Should().Be(2);
        pagedEntities[1].Id.Should().Be(4);
    }

    [Fact]
    public void Applies_Paging_With_OrderBy()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Charlie", Value = 30 },
            new() { Id = 2, Name = "Alpha", Value = 10 },
            new() { Id = 3, Name = "Delta", Value = 40 },
            new() { Id = 4, Name = "Bravo", Value = 20 },
        };

        var specification = new TestEntityOrderByNameWithPagingSpecification(1, 2);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(2);
        pagedEntities[0].Name.Should().Be("Bravo");
        pagedEntities[1].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Applies_Paging_With_Filter_And_OrderBy()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Charlie", Value = 20 },
            new() { Id = 2, Name = "Alpha", Value = 10 },
            new() { Id = 3, Name = "Delta", Value = 20 },
            new() { Id = 4, Name = "Bravo", Value = 20 },
            new() { Id = 5, Name = "Echo", Value = 20 },
        };

        var specification = new TestEntityByValueWithOrderByNameAndPagingSpecification(20, 1, 2);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(2);
        pagedEntities[0].Name.Should().Be("Charlie");
        pagedEntities[1].Name.Should().Be("Delta");
    }

    [Fact]
    public void No_Paging_Returns_All_Entities()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
        };

        var specification = new TestEntityNoPagingSpecification();

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().HaveCount(3);
    }

    [Fact]
    public void Take_Zero_Returns_Empty()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
        };

        var specification = new TestEntityPaginateTakeOnlySpecification(0);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().BeEmpty();
    }

    [Fact]
    public void Paging_On_Empty_Collection_Returns_Empty()
    {
        var entities = new List<TestEntity>();

        var specification = new TestEntityPaginateSpecification(0, 10);

        var pagedEntities = entities.Filter(specification).ToList();

        pagedEntities.Should().BeEmpty();
    }

    [Fact]
    public void Applies_Multiple_Pages_Correctly()
    {
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "First", Value = 10 },
            new() { Id = 2, Name = "Second", Value = 20 },
            new() { Id = 3, Name = "Third", Value = 30 },
            new() { Id = 4, Name = "Fourth", Value = 40 },
            new() { Id = 5, Name = "Fifth", Value = 50 },
            new() { Id = 6, Name = "Sixth", Value = 60 },
        };

        var page1Spec = new TestEntityPaginateSpecification(0, 2);
        var page2Spec = new TestEntityPaginateSpecification(2, 2);
        var page3Spec = new TestEntityPaginateSpecification(4, 2);

        var page1 = entities.Filter(page1Spec).ToList();
        var page2 = entities.Filter(page2Spec).ToList();
        var page3 = entities.Filter(page3Spec).ToList();

        page1.Should().HaveCount(2);
        page1[0].Id.Should().Be(1);
        page1[1].Id.Should().Be(2);

        page2.Should().HaveCount(2);
        page2[0].Id.Should().Be(3);
        page2[1].Id.Should().Be(4);

        page3.Should().HaveCount(2);
        page3[0].Id.Should().Be(5);
        page3[1].Id.Should().Be(6);
    }

    private class TestEntity : Entity
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class TestEntityPaginateSpecification : Specification<TestEntity>
    {
        public TestEntityPaginateSpecification(int skip, int take)
        {
            Paginate(skip, take);
        }
    }

    private class TestEntityPaginateSkipOnlySpecification : Specification<TestEntity>
    {
        public TestEntityPaginateSkipOnlySpecification(int skip)
        {
            Paginate(skip, int.MaxValue);
        }
    }

    private class TestEntityPaginateTakeOnlySpecification : Specification<TestEntity>
    {
        public TestEntityPaginateTakeOnlySpecification(int take)
        {
            Paginate(0, take);
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

    private class TestEntityOrderByNameWithPagingSpecification : Specification<TestEntity>
    {
        public TestEntityOrderByNameWithPagingSpecification(int skip, int take)
        {
            OrderBy(e => e.Name);
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

    private class TestEntityNoPagingSpecification : Specification<TestEntity>
    {
        public TestEntityNoPagingSpecification()
        {
        }
    }
}
