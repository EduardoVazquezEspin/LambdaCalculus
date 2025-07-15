using LambdaCalculus;

namespace LambdaCalculusTests.lambda;

public class LambdaSimplifyDevelopmentTests
{
    [TestCase("([{([{λA.A}])}])", "λA.A")] // Parenthesis of everything is unnecessary
    [TestCase("λA.λB.A ([{([{B A}])}])", "λA.λB.A (B A)")] // Parenthesis of parenthesis is unnecessary
    [TestCase("λx.(x x)", "λx.x x")] // Lambda of Parenthesis is lambda
    [TestCase("λx.x (x)", "λx.x x")] // Parenthesis of single variable is unnecessary
    [TestCase("λx.λy.λz.(x y) x", "λx.λy.λz.x y x")] // Composition is left associative
    [TestCase("λx.λy.x {λz.y x}", "λx.λy.x λz.y x")] // Lambdas extend to the right
    [TestCase("[λf.f f] {λx.λy.x} (λx.λy.y)", "[λf.f f] {λx.λy.x} λx.λy.y")] // Lambdas extend to the right
    [TestCase("[[λx.λy.{{([{([{((x)) [y]}])}]) {λz.z}}}]]", "λx.λy.x y λz.z")] // A bit of everything
    public void LambdaSimplify_RunsSuccessfully_AndReturnsSimplerLambda(string expression, string simplified)
    {
        var lambda = new LambdaParser().ParseExpression(expression, out var error)!;
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda.ToString(), Is.EqualTo(simplified));
        Assert.True(lambda.IsWellFormatted());
    }
}