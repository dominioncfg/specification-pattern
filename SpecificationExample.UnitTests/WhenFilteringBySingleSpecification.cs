using SpecificationExample.Domain.Common;

namespace SpecificationExample.UnitTests
{
    public class WhenFilteringBySingleSpecification
    {
        [Fact]
        public void Test1()
        {

        }



        private class TestEntity : Entity
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        private class TestEntityByNameSpecification : Specification<TestEntity>
        {
            public TestEntityByNameSpecification(string name)
            {
                Rule(e => e.Name == name);
            }
        }
    }
}
