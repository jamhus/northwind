namespace Northwind.Dashboard.Engine
{
    public class ParameterStore
    {
        private readonly Dictionary<string, object> _data = new(StringComparer.OrdinalIgnoreCase);

        public void Set(string key, object? value) => _data[key] = value;
        public object Get(string key) => _data.TryGetValue(key, out var value) ? value : null;

        public T? Get<T> (string key)
        {
            var value = Get(key);
            if (value is null) return default;
            return (T?)Convert.ChangeType(value, typeof(T));
        }

        public IReadOnlyDictionary<string, object?> AsReadOnly() => _data;
    }
}
