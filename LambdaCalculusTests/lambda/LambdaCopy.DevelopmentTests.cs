using LambdaCalculus;

namespace LambdaCalculusTests.lambda;

public class LambdaCopyDevelopmentTests
{
    [TestCase("λx.x")]
    [TestCase("λx.λy.x")]
    [TestCase("λx.λx.x")]
    [TestCase("λx.(λx.x) x")] // η reduction
    [TestCase("λn.λm.λf.λx.n f (m f x)")]
    [TestCase("λn.λm.λf.λx.n f (m (f x))")]
    [TestCase("[λf.f f] {λx.λy.x} λx.λy.y")]
    [TestCase("(λx.x) (λx.x) (λx.x) (λx.x) λx.x")]
    [TestCase("(λx.x) (λx.x) (λx.x) [(λx.x) λx.x]")]
    [TestCase("(λx.x) (λx.x) [(λx.x) λx.x] λx.x")]
    [TestCase("(λx.x) (λx.x) [(λx.x) (λx.x) λx.x]")]
    [TestCase("(λx.x) (λx.x) [(λx.x) [(λx.x) λx.x]]")]
    [TestCase("(λx.x) [(λx.x) λx.x] (λx.x) λx.x")]
    [TestCase("(λx.x) [(λx.x) λx.x] [(λx.x) λx.x]")]
    [TestCase("(λx.x) [(λx.x) (λx.x) λx.x] λx.x")]
    [TestCase("(λx.x) [(λx.x) [(λx.x) λx.x]] λx.x")]
    [TestCase("(λx.x) [(λx.x) (λx.x) (λx.x) λx.x]")]
    [TestCase("(λx.x) [(λx.x) (λx.x) [(λx.x) λx.x]]")]
    [TestCase("(λx.x) [(λx.x) [(λx.x) λx.x] λx.x]")]
    [TestCase("(λx.x) [(λx.x) [(λx.x) (λx.x) λx.x]]")]
    [TestCase("(λx.x) [(λx.x) [(λx.x) [(λx.x) λx.x]]]")]
    public void LambdaCopy_RunsSuccessfully_AndReturnsAValidExpression(string expressionStr)
    {
        var expression = ExpressionParser.ParseExpression(expressionStr)!;
        var copy = expression.Copy();
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
    }

    [TestCase("λx.x", 1)]
    [TestCase("λx.x x", 2)]
    [TestCase("λx.λy.x λx.x", 1)]
    [TestCase("λx.λy.x (λx.x) x", 2)]
    public void LambdaCopy_RunsSuccessfully_KeepsCountOfVariableCalls(string expressionStr, int expectedCalls)
    {
        var expression = ExpressionParser.ParseLambda(expressionStr)!;
        var copy = expression.Copy();
        Assert.That(expression.Definition.Calls, Is.EqualTo(expectedCalls));
        Assert.That(copy.Definition.Calls, Is.EqualTo(expectedCalls));
    }
    
    [Test]
    public void LambdaCopy_RunsSuccessfully_MaintainsVariableReference1()
    {
        var expressionStr = "λx.x";
        var expression = ExpressionParser.ParseLambda(expressionStr, out var error)!;
        Assert.That(error, Is.InstanceOf<NoError>());
        var copy = expression.Copy();
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
        Assert.That(expression.Definition, Is.EqualTo((expression.Expression as Variable)!.Definition));
        Assert.That(copy.Definition, Is.EqualTo((copy.Expression as Variable)!.Definition));
        Assert.That(expression.Definition, Is.Not.EqualTo(copy.Definition));
    }
    
    [Test]
    public void LambdaCopy_RunsSuccessfully_MaintainsVariableReference2()
    {
        var expressionStr = "λx.λy.x";
        var expression = ExpressionParser.ParseLambda(expressionStr, out var error)!;
        Assert.That(error, Is.InstanceOf<NoError>());
        var copy = expression.Copy();
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
        Assert.That(expression.Definition, Is.EqualTo(((expression.Expression as Lambda)!.Expression as Variable)!.Definition));
        Assert.That(copy.Definition, Is.EqualTo(((copy.Expression as Lambda)!.Expression as Variable)!.Definition));
        Assert.That(expression.Definition, Is.Not.EqualTo(copy.Definition));
    }
    
    [Test]
    public void LambdaCopy_RunsSuccessfully_MaintainsVariableReference3()
    {
        var expressionStr = "λx.λy.y";
        var expression = ExpressionParser.ParseLambda(expressionStr, out var error)!;
        var childLambda = (Lambda) expression.Expression;
        Assert.That(error, Is.InstanceOf<NoError>());
        var copy = expression.Copy();
        var childCopy = (Lambda) copy.Expression;
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
        Assert.That(childLambda.Definition, Is.EqualTo((childLambda.Expression as Variable)!.Definition));
        Assert.That(childCopy.Definition, Is.EqualTo((childCopy.Expression as Variable)!.Definition));
        Assert.That(childLambda.Definition, Is.Not.EqualTo(childCopy.Definition));
    }
}