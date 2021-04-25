using System;
using System.Collections.Concurrent;
using TestTask.Repo.Interfaces;

namespace TestTask.Repo.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ConcurrentDictionary<Guid, T> _dictionary { get; } = new();

        public T GetEntry(Guid id)
        {
            _dictionary.TryGetValue(id, out T value);
            return value;
        }

        public bool HasEntry(Guid id) => _dictionary.TryGetValue(id, out T value);

        public T SetOrUpdateEntry(Guid id, T value)
        {
            _dictionary.TryGetValue(id, out T oldValue);

            return _dictionary.AddOrUpdate(id,
                id => value,
                (id, oldValue) => value);
        }
    }
}
