using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]

[TestClass]
public class CacheTests
{
    [TestMethod]
    public void Add_NewItems_StoresValuesAndUpdatesCount()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("one", 1);
        cache.Add("two", 2);

        Assert.AreEqual(2, cache.Count);
        Assert.AreEqual(1, cache.Get("one"));
        Assert.AreEqual(2, cache.Get("two"));
    }

    [TestMethod]
    public void Add_ExistingKey_UpdatesValueWithoutChangingFifoPosition()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("one", 1);
        cache.Add("two", 2);

        cache.Add("one", 10);
        Assert.AreEqual(10, cache.Get("one"));
        cache.Add("three", 3);

        Assert.IsFalse(cache.ContainsKey("one"));
        Assert.IsTrue(cache.ContainsKey("two"));
        Assert.IsTrue(cache.ContainsKey("three"));
        Assert.AreEqual(2, cache.Count);
    }

    [TestMethod]
    public void Add_WhenCapacityIsExceeded_EvictsOldestItem()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");

        cache.Add(3, "three");

        Assert.IsFalse(cache.ContainsKey(1));
        Assert.AreEqual("two", cache.Get(2));
        Assert.AreEqual("three", cache.Get(3));
        Assert.AreEqual(2, cache.Count);
    }

    [TestMethod]
    public void Add_AfterEvictionAndReinsertion_MaintainsFifoOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Add(3, "three");
        cache.Add(1, "one again");

        Assert.IsFalse(cache.ContainsKey(2));
        Assert.IsTrue(cache.ContainsKey(3));
        Assert.AreEqual("one again", cache.Get(1));
    }

    [TestMethod]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, int>(1);

        Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
    }

    [TestMethod]
    public void ContainsKey_ReflectsCacheContents()
    {
        var cache = new Cache<string, int>(1);
        cache.Add("present", 1);

        Assert.IsTrue(cache.ContainsKey("present"));
        Assert.IsFalse(cache.ContainsKey("absent"));
    }

    [TestMethod]
    public void Constructor_NonPositiveCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<System.ArgumentOutOfRangeException>(() => new Cache<int, int>(0));
    }
}
