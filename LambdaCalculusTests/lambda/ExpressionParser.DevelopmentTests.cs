using LambdaCalculus.lambda;

namespace LambdaCalculusTests.lambda;

public class ExpressionParserDevelopmentTests
{
    [SetUp]
    public void Setup() { }
    
    [TestCase("λx.x")]
    [TestCase("λx.λy.x")]
    [TestCase("λx.λx.x")]
    [TestCase("λn.λm.λf.λx.n f (m f x)")]
    [TestCase("λn.λm.λf.λx.n f (m (f x))")]
    [TestCase("[λf.f f] {λx.λy.x} λx.λy.y")]
    [TestCase("([{λA.A}])")]
    public void ExpressionParser_ParsesSuccessfully_AndStringifiesEqualToInput(string expression)
    {
        var parser = new ExpressionParser();
        var lambda = parser.Parse(expression);
        Assert.That(lambda.ToString(), Is.EqualTo(expression));
    }
}