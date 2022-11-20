[![.NET](https://github.com/aimenux/LazyConcurrentDictionaryDemo/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/aimenux/LazyConcurrentDictionaryDemo/actions/workflows/ci.yml)

# LazyConcurrentDictionaryDemo
```
Using lazy concurrent dictionary to ensure thread safety for unsafe methods
```

> In this repo, i m comparing two implementations of concurrent dictionary when dealing with methods like [GetOrAdd](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.getoradd) or [AddOrUpdate](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.addorupdate) :
>
> - `DefaultConcurrentDictionary` : use default implementation of concurrent dictionary, methods like `GetOrAdd` or `AddOrUpdate` are not thread safe (see [link](https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/))
>
> - `LazyConcurrentDictionary` : use custom implementation of concurrent dictionary, methods like `GetOrAdd` or `AddOrUpdate` are thread safe (see [link](https://andrewlock.net/making-getoradd-on-concurrentdictionary-thread-safe-using-lazy/))
>
> ![LazyConcurrentDictionaryDemo](screenshots/LazyConcurrentDictionaryDemo.png)
>

**`Tools`** : vs22, net 6.0
