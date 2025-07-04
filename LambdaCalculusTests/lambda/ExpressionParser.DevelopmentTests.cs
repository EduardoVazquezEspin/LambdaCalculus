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
        var lambda = parser.ParseExpression(expression, out var error);
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda!.ToString(), Is.EqualTo(expression));
    }
    
    [TestCase("λx.",typeof(UnfinishedExpression))]
    [TestCase("λλx.x]", typeof(InvalidCharacter))]
    [TestCase("λxλ.x]", typeof(InvalidCharacter))]
    [TestCase("λ.x.x]", typeof(InvalidCharacter))]
    [TestCase("λx..x]", typeof(InvalidCharacter))]
    [TestCase("λx.λy.(x]", typeof(InvalidCharacter))]
    [TestCase("", typeof(EmptyExpression))]
    [TestCase("([])",typeof(UnfinishedExpression))]
    [TestCase("([)]", typeof(UnfinishedExpression))]
    [TestCase("([λx.x)]", typeof(InvalidCharacter))]
    public void ExpressionParser_ParsesUnsuccessfully_ReturnsNullAndError(string expression, Type type)
    {
        var parser = new ExpressionParser();
        var lambda = parser.ParseExpression(expression, out var error);
        Assert.That(error.GetType(), Is.EqualTo(type));
        Assert.Null(lambda);
    }
}