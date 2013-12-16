using NUnit.Framework;
using UnityEngine;

//See NUnit docs for full examples: http://www.nunit.org/index.php?p=assertions&r=2.2.10

[TestFixture]
public class TestFixtureA
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
    public void TestNotNull()
    {
        Object a = new Object();
        Assert.IsNotNull(a, "object should not be null");
    }


    [Test]
    [Ignore("This is an ignored test")]
    public void IgnoredTest()
    {
    }

    
}
