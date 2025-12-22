Feature: In-Memory Specification
    Simple specification to demonstrate in-memory testing.

Scenario: Can Filter Blogs by name in Memory
	Given the existing blogs in memory:
		| Id | Name         |
		|  1 | Test Blog 1  |
		|  2 | Sample Blog  |
		|  3 | Another Blog |
	When I apply the specification with name 'Test Blog 1' in memory
	Then the following blogs are returned:
		| Id | Name        |
		|  1 | Test Blog 1 |
