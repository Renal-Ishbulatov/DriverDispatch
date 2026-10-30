using DriverDispatch.Core.Models;
using NUnit.Framework;

namespace DriverDispatch.Tests;

[TestFixture]
public class ModelsTests
{
    [Test]
    public void Driver_Equality_ByValue()
    {
        var a = new Driver(1, 5, 5);
        var b = new Driver(1, 5, 5);
        Assert.That(a, Is.EqualTo(b));
        Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
    }

    [Test]
    public void Order_Equality_ByValue()
    {
        var a = new Order(3, 4);
        var b = new Order(3, 4);
        Assert.That(a, Is.EqualTo(b));
    }

    [Test]
    public void Driver_IsValueType()
    {
        var a = new Driver(1, 5, 5);
        var b = a;
        Assert.That(a.Equals(b), Is.True);
    }
}