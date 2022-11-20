namespace Lib;

public interface IConcurrentDictionary<TKey, TValue>
{
    TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
}