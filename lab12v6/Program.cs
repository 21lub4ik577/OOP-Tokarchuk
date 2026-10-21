using lab12v6;

var repository = new InMemoryRepository<Employee>();

var firstEmployee = new Employee
{
    Id = 1,
    FirstName = "Семен",
    LastName = "Шишка",
    Position = "Розробник"
};
var secondEmployee = new Employee
{
    Id = 2,
    FirstName = "Володя",
    LastName = "Відерко",
    Position = "Тестувальник"
};

repository.Add(firstEmployee);
repository.Add(secondEmployee);

Console.WriteLine("Працівники:");
foreach (var employee in repository.GetAll())
{
    Console.WriteLine($"{employee.Id}: {employee.FirstName} {employee.LastName} — {employee.Position}");
}

repository.Update(new Employee
{
    Id = 1,
    FirstName = "Семен",
    LastName = "Шишка",
    Position = "Старший розробник"
});
Console.WriteLine($"\nОновлена посада: {repository.GetById(1).Position}");

repository.Delete(2);
Console.WriteLine($"Кількість після видалення: {repository.GetAll().Count()}");
