using FluentAssertions;
using Reqnroll;
using SpecificationExample.Domain.Blogs;
using SpecificationExample.Domain.Common;

namespace SpecificationExample.AcceptanceTests.Features.StepDefinitions
{
    [Binding]
    public class SpecificationBlogsStepsDefinitions
    {
        private List<Blog> givenBlogs = [];
        private List<Blog> actualBlogs = [];

        [Given(@"the existing blogs in memory:")]
        public void GivenIHaveEnteredIntoTheCalculator(Table table)
        {
            givenBlogs = GetBlogsFromTable(table).Select(x=> new Blog()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        [When(@"I apply the specification with name '(.*)' in memory")]
        public void WhenIApplyTheSpecificationWithName(string name)
        {
           var _specification = new FirstTenBlogByNameSpecification(name);
            actualBlogs = [.. givenBlogs.Filter(_specification)];
        }

        [Then(@"the following blogs are returned:")]
        public void TheExistingBlogsShouldHaveTheFollowingData(Table table)
        {
            var expectedBlogs = GetBlogsFromTable(table);

            actualBlogs.Count.Should().Be(expectedBlogs.Count);

            foreach (var expectedBlog in expectedBlogs)
            {
                var actualBlog = givenBlogs.FirstOrDefault(b => b.Id == expectedBlog.Id);    
                actualBlog.Should().NotBeNull();
                actualBlog.Name.Should().Be(expectedBlog.Name);
            }
        }

        private List<BlogRowDto> GetBlogsFromTable(Table table)
        {
            return [.. table.CreateSet<BlogRowDto>()];
        }

        private class BlogRowDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
