using LambdaCalculus.lambda;

namespace LambdaCalculusTests.lambda;

public class LambdaSimplifyDevelopmentTests
{
    [TestCase("([{([{λA.A}])}])", "λA.A")]
    [TestCase("λA.λB.A ([{([{B A}])}])", "λA.λB.A (B A)")]
    [TestCase("λx.(x x)", "λx.x x")]
    [TestCase("λx.x (x)", "λx.x x")]
    [TestCase("λx.λy.λz.(x y) z", "λx.λy.λz.x y z")]
    [TestCase("[[λx.λy.λz.{{([{([{((x)) [y]}])}]) {z}}}]]", "λx.λy.λz.x y z")]
    [TestCase("λx.λy.x {λz.y z}", "λx.λy.x λz.y z")]
    public void LambdaSimplify_RunsSuccessfully_AndReturnsSimplerLambda(string expression, string simplified)
    {
        var parser = new ExpressionParser();
        var lambda = parser.ParseExpression(expression, out var error)!.Simplify();
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda!.ToString(), Is.EqualTo(simplified));
        Assert.True(lambda.IsWellFormatted());
    }
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference1()
    {
        var expression = "λx.x";
        var parser = new ExpressionParser();
        var lambda = parser.ParseLambda(expression, out var error)!.Simplify() as Lambda;
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda!.Variable, Is.EqualTo(lambda.Expression));
        Assert.True(lambda.IsWellFormatted());
    }
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference2()
    {
        var expression = "λx.λy.x";
        var parser = new ExpressionParser();
        var lambda = parser.ParseLambda(expression, out var error)!.Simplify() as Lambda;
        var childLambda = lambda!.Expression as Lambda;
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda.Variable, Is.EqualTo(childLambda!.Expression));
        Assert.True(lambda.IsWellFormatted());
    }
    
    [Test]
    public void LambdaSimplify_RunsSuccessfully_MaintainsVariableReference3()
    {
        var expression = "λx.λx.x";
        var parser = new ExpressionParser();
        var lambda = parser.ParseLambda(expression, out var error)!.Simplify() as Lambda;
        var childLambda = lambda!.Expression as Lambda;
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(childLambda!.Variable, Is.EqualTo(childLambda.Expression));
        Assert.True(lambda.IsWellFormatted());
    }
}