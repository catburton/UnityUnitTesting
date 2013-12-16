using NUnit.Framework;
using UnityEngine;

//See NUnit docs for full examples: http://www.nunit.org/index.php?p=assertions&r=2.2.10

[TestFixture]
public class TestFixtureC
{
    [SetUp]
    public void SetUp()
    {
        //This attribute is used inside a TestFixture to provide a common set of 
        //functions that are performed just before each test method is called
    }

    [TearDown]
    public void TearDown()
    {
        //This attribute is used inside a TestFixture to provide a common set of 
        //functions that are performed after each test method is run.
    }

    [Test]
    public void IntegerValueTest()
    { 
        Assert.That(42, Is.GreaterThan(40) & Is.LessThan(45));
    }

    [Test]
    public void EqualityTest()
    {
        Assert.AreEqual(5, 5, "Values should be equal");
    }

    [Test]
    public void NotEqualsTest()
    {
        Assert.AreNotEqual(5, 7, "Values should not be equal");
    }

    [Test]
    public void VariablesReferenceSameObjectTest()
    {
        Object a = new Object();
        Object b = a;
        Assert.AreSame(a, b, "The two variables should reference same object");
    }

    [Test]
    public void VariablesDoNotReferenceSameObjectTest()
    {
        Object a = new Object();
        Object b = new Object();
        Assert.AreNotSame(a, b, "The two variables should not reference same object");
    }

    [Test]
    public void TestDoesNotThrowException()
    {
        Assert.DoesNotThrow(DelegateDoesNotThrowException, "function should not cause an Exception");
        Assert.Throws<System.NotImplementedException>(DelegateThrowsException, "function should throw exception");
    }

    void DelegateThrowsException()
    {
        throw new System.NotImplementedException();
    }

    void DelegateDoesNotThrowException()
    {
    }

    [Test]
    public void TestFalseCondition()
    {
        Assert.False(false, "value should be false");
    }

    [Test]
    public void TestTrueCondition()
    {
        Assert.True(true, "value should be true");
    }
     
    [Test]
    public void TestNotNull()
    {
        Object a = new Object();
        Assert.IsNotNull(a, "object should not be null");
    }

    [Test]
    public void TestIsNull()
    {
        Object a = null;
        Assert.IsNull(a, "object should be null");

    }

    [Test]
    [Ignore("Description of ignored test")]
    public void IgnoredTest()
    {
    }

    [Test]
    [ExpectedException(typeof(UnityException))]
    public void TestExpectedException()
    {
        throw new UnityException("Test exception");
    }
    
}
