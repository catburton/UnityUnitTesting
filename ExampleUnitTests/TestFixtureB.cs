using NUnit.Framework;
using UnityEngine;

//See NUnit docs for full examples: http://www.nunit.org/index.php?p=assertions&r=2.2.10
[TestFixture]
public class TestFixtureB
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
        Assert.That(1, Is.GreaterThan(5), "Example failing test");
    }

    [Test]
    public void MultipleAsserts()
    {
        Assert.True(true);
        Assert.False(false);
    }

    [Test]
    public void FailingAssert()
    {
        Assert.True(false, "Test value should be true");
    }
}