using FluentAssertions;
using Lib;
using Xunit.Abstractions;

namespace Tests;

public class LazyConcurrentDictionaryTests
{
    private readonly ValueFactory _valueFactory;

    public LazyConcurrentDictionaryTests(ITestOutputHelper outputHelper)
    {
        _valueFactory = new ValueFactory(outputHelper);
    }

    [Fact]
    public void Given_Multiple_Threads_Then_ValueFactory_Run_One_Time_V1()
    {
        // arrange
        var dictionary = new LazyConcurrentDictionary<string, string>();

        // act
        var task1 = Task.Run(() => dictionary.GetOrAdd("SomeKey", _valueFactory.CreateValueForKey));
        var task2 = Task.Run(() => dictionary.GetOrAdd("SomeKey", _valueFactory.CreateValueForKey));
        Task.WaitAll(task1, task2);
        var value1 = task1.Result;
        var value2 = task2.Result;

        // assert
        value1.Should().Be(value2);
        _valueFactory.Counter.Should().Be(1);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void Given_Multiple_Threads_Then_ValueFactory_Run_One_Time_V2(int retries)
    {
        // arrange
        var dictionary = new LazyConcurrentDictionary<string, string>();

        // act
        Parallel.For(0, retries, _ =>
        {
            dictionary.GetOrAdd("SomeKey", _valueFactory.CreateValueForKey);
        });

        // assert
        _valueFactory.Counter.Should().Be(1);
    }

    private class ValueFactory
    {
        internal int Counter = 0;

        private readonly ITestOutputHelper _outputHelper;

        internal ValueFactory(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        internal string CreateValueForKey(string key)
        {
            var value = Guid.NewGuid().ToString("D");
            _outputHelper.WriteLine($"Create value '{value}' for key '{key}'");
            Thread.Sleep(TimeSpan.FromMilliseconds(10));
            Interlocked.Increment(ref Counter);
            return value;
        }
    }
}