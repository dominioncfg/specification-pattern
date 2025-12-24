Feature: In-Memory Specification
    Simple specification to demonstrate in-memory testing.

Scenario: Can Filter Blogs by name in Memory
	Given the existing blogs in memory:
		| Id       | Name         |
		| @Blog1Id | Test Blog 1  |
		| @Blog2Id | Sample Blog  |
		| @Blog3Id | Another Blog |
	When I apply the specification with name 'Test Blog 1' in memory
	Then the following blogs are returned:
		| Id       | Name        |
		| @Blog1Id | Test Blog 1 |
