using LambdaCalculus.helpers;

namespace LambdaCalculusTests.helpers;

public class QuotientTests
{
    // We are going to color the abecedary
    private Quotient<char> _quotient = null!;
    
    [SetUp]
    public void Setup()
    {
        _quotient = new Quotient<char>('a', 'b', 'c', 'd');
    }
    
    [Test]
    public void ColorMapTotalColor1()
    {
        _quotient.SetClass('a');
        Assert.That(_quotient.ClassCount, Is.EqualTo(4));
    }

    [Test]
    public void ColorMapTotalColor2()
    {
        _quotient.SetEqual('a', 'b');
        Assert.That(_quotient.ClassCount, Is.EqualTo(3));
    }
    
    [Test]
    public void ColorMapTotalColor3()
    {
        _quotient.SetEqual('a', 'b');
        _quotient.SetEqual('c', 'd');
        _quotient.SetEqual('d', 'a');
        Assert.That(_quotient.AreEqual('b', 'c'), Is.True);
        Assert.That(_quotient.ClassCount, Is.EqualTo(1));
    }
}