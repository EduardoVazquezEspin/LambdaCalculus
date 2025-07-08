using LambdaCalculus.lambda;

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
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference1()
    {
        var expressionStr = "λx.x";
        var expression = ExpressionParser.ParseLambda(expressionStr, out var error)!;
        Assert.That(error, Is.InstanceOf<NoError>());
        var copy = expression.Copy();
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
        Assert.That(expression.Variable, Is.EqualTo(expression.Expression));
        Assert.That(copy.Variable, Is.EqualTo(copy.Expression));
        Assert.That(expression.Variable, Is.Not.EqualTo(copy.Variable));
    }
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference2()
    {
        var expressionStr = "λx.λy.x";
        var expression = ExpressionParser.ParseLambda(expressionStr, out var error)!;
        Assert.That(error, Is.InstanceOf<NoError>());
        var copy = expression.Copy();
        Assert.True(expression.IsWellFormatted());
        Assert.True(copy.IsWellFormatted());
        Assert.That(copy.ToString(), Is.EqualTo(expression.ToString()));
        Assert.That(expression.Variable, Is.EqualTo((expression.Expression as Lambda)!.Expression));
        Assert.That(copy.Variable, Is.EqualTo((copy.Expression as Lambda)!.Expression));
        Assert.That(expression.Variable, Is.Not.EqualTo(copy.Variable));
    }
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference3()
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
        Assert.That(childLambda.Variable, Is.EqualTo(childLambda.Expression));
        Assert.That(childCopy.Variable, Is.EqualTo(childCopy.Expression));
        Assert.That(childLambda.Variable, Is.Not.EqualTo(childCopy.Variable));
    }
}