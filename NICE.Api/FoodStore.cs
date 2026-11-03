using System.Text.Json;

namespace NICE.Api;

/// Datenhaltung des Backends: Liste im Speicher, bei jeder Änderung in foods.json gesichert.
public class FoodStore
{
    private readonly object _lock = new();
    private readonly string _file = Path.Combine(AppContext.BaseDirectory, "foods.json");
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web) { WriteIndented = true };
    private readonly List<FoodItem> _items;

    public FoodStore()
    {
        _items = File.Exists(_file)
            ? JsonSerializer.Deserialize<List<FoodItem>>(File.ReadAllText(_file), _json) ?? new()
            : new();
    }

    public List<FoodItem> GetAll()
    {
        lock (_lock) return _items.OrderByDescending(i => i.EatenAt).ToList();
    }

    public FoodItem? Get(Guid id)
    {
        lock (_lock) return _items.FirstOrDefault(i => i.Id == id);
    }

    public FoodItem Add(FoodItem item)
    {
        lock (_lock)
        {
            item.Id = Guid.NewGuid();      // Der Server vergibt die ID
            _items.Add(item);
            Save();
            return item;
        }
    }

    public bool Update(Guid id, FoodItem item)
    {
        lock (_lock)
        {
            var index = _items.FindIndex(i => i.Id == id);
            if (index < 0) return false;
            item.Id = id;
            _items[index] = item;
            Save();
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var removed = _items.RemoveAll(i => i.Id == id) > 0;
            if (removed) Save();
            return removed;
        }
    }

    private void Save() => File.WriteAllText(_file, JsonSerializer.Serialize(_items, _json));
}