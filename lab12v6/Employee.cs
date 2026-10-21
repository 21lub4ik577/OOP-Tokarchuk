namespace lab12v6;

public sealed class Employee : IEntity
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;
}
