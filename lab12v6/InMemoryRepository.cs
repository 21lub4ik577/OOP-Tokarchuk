namespace lab12v6;

public sealed class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = [];

    public T GetById(int id)
    {
        var index = _items.FindIndex(candidate => candidate.Id == id);
        if (index < 0)
        {
            throw new KeyNotFoundException($"Entity with ID {id} was not found.");
        }

        return _items[index];
    }

    public IEnumerable<T> GetAll() => _items.ToList();

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_items.Exists(candidate => candidate.Id == item.Id))
        {
            throw new InvalidOperationException($"An entity with ID {item.Id} already exists.");
        }

        _items.Add(item);
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var index = _items.FindIndex(candidate => candidate.Id == item.Id);
        if (index < 0)
        {
            throw new KeyNotFoundException($"Entity with ID {item.Id} was not found.");
        }

        _items[index] = item;
    }

    public void Delete(int id)
    {
        var index = _items.FindIndex(candidate => candidate.Id == id);
        if (index < 0)
        {
            throw new KeyNotFoundException($"Entity with ID {id} was not found.");
        }

        _items.RemoveAt(index);
    }
}
