using lab12v6;

namespace lab12v6.Tests;

public class InMemoryRepositoryTests
{
    [Fact]
    public void Add_AddsNewEmployee()
    {
        var repository = new InMemoryRepository<Employee>();
        var employee = CreateEmployee(1);

        repository.Add(employee);

        Assert.Same(employee, repository.GetById(1));
    }

    [Fact]
    public void Add_ThrowsWhenEmployeeIdAlreadyExists()
    {
        var repository = new InMemoryRepository<Employee>();
        repository.Add(CreateEmployee(1));

        Assert.Throws<InvalidOperationException>(() => repository.Add(CreateEmployee(1)));
    }

    [Fact]
    public void GetById_ReturnsEmployeeWithMatchingId()
    {
        var repository = new InMemoryRepository<Employee>();
        var employee = CreateEmployee(1);
        repository.Add(employee);

        Assert.Same(employee, repository.GetById(1));
    }

    [Fact]
    public void GetById_ThrowsWhenEmployeeDoesNotExist()
    {
        var repository = new InMemoryRepository<Employee>();

        Assert.Throws<KeyNotFoundException>(() => repository.GetById(99));
    }

    [Fact]
    public void GetAll_ReturnsAllEmployees()
    {
        var repository = new InMemoryRepository<Employee>();
        var first = CreateEmployee(1);
        var second = CreateEmployee(2);
        repository.Add(first);
        repository.Add(second);

        Assert.Equal(new[] { first, second }, repository.GetAll());}

    [Fact]
    public void Update_ReplacesExistingEmployee()
    {
        var repository = new InMemoryRepository<Employee>();
        repository.Add(CreateEmployee(1));
        var updatedEmployee = new Employee
        {
            Id = 1,
            FirstName = "Updated",
            LastName = "Employee",
            Position = "Manager"
        };

        repository.Update(updatedEmployee);

        Assert.Same(updatedEmployee, repository.GetById(1));
        Assert.Equal("Manager", repository.GetById(1).Position);
    }

    [Fact]
    public void Update_ThrowsWhenEmployeeDoesNotExist()
    {
        var repository = new InMemoryRepository<Employee>();

        Assert.Throws<KeyNotFoundException>(() => repository.Update(CreateEmployee(99)));
    }

    [Fact]
    public void Delete_RemovesExistingEmployee()
    {
        var repository = new InMemoryRepository<Employee>();
        repository.Add(CreateEmployee(1));

        repository.Delete(1);

        Assert.Empty(repository.GetAll());
        Assert.Throws<KeyNotFoundException>(() => repository.GetById(1));
    }

    [Fact]
    public void Delete_ThrowsWhenEmployeeDoesNotExist()
    {
        var repository = new InMemoryRepository<Employee>();

        Assert.Throws<KeyNotFoundException>(() => repository.Delete(99));
    }

    private static Employee CreateEmployee(int id) => new()
    {
        Id = id,
        FirstName = "Іван",
        LastName = "Виделка",
        Position = "Інженер"
    };
}
