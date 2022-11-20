using System.Collections.Concurrent;

namespace Lib;

public class DefaultConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
{
    private readonly ConcurrentDictionary<TKey, TValue> _concurrentDictionary;

    public DefaultConcurrentDictionary()
    {
        _concurrentDictionary = new ConcurrentDictionary<TKey, TValue>();
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        return _concurrentDictionary.GetOrAdd(key, valueFactory(key));
    }
}