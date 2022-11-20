using System.Collections.Concurrent;

namespace Lib;

public class LazyConcurrentDictionary<TKey, TValue> : IConcurrentDictionary<TKey, TValue>
{
    private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _concurrentDictionary;

    public LazyConcurrentDictionary()
    {
        _concurrentDictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
    }

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
    {
        var lazyValueFactory = new Lazy<TValue>(() => valueFactory(key), LazyThreadSafetyMode.ExecutionAndPublication);
        var lazyValue = _concurrentDictionary.GetOrAdd(key, lazyValueFactory);
        return lazyValue.Value;
    }
}