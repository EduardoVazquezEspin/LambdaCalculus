using LambdaCalculus.lambda;

namespace LambdaCalculusTests.lambda;

public class EtaReductionDevelopmentTests
{
    [TestCase("λy.y (λx.y y y x) λx.λy.x", "λy.y (y y y) λx.λy.x")]
    [TestCase("λx.x λy.λx.y y y x", "λx.x λy.y y y")]
    [TestCase("λy.(λx.y y y x) λx.λy.x", "λy.y y y λx.λy.x")]
    [TestCase("λx.(λy.y y) (λy.y y) x", "(λy.y y) λy.y y")]
    [TestCase("λx.λy.x y λz.x y z", "λx.λy.x y (x y)")]
    [TestCase("λx.x λy.x y", "λx.x x")]
    [TestCase("λx.(λy.x y) x", "λx.x x")]
    [TestCase("λx.λy.(λx.x x) y", "λx.λx.x x")]
    [TestCase("λx.x λy.(λx.x x) y", "λx.x λx.x x")]
    [TestCase("λx.x (λy.(λx.x x) y) x", "λx.x (λx.x x) x")]
    [TestCase("λa.λx.a λy.x y", "λa.a")]
    [TestCase("λa.(λb.a λx.(λy.b y) x)", "λa.a")] // Only example that requires the do while loop
    [TestCase("λx.(λx.x) x", "λx.x")] // η reduction
    [TestCase("λx.λy.λz.(x y) z", "λx.x")] // η reduction
    [TestCase("λx.λy.x λz.x y z", "λx.λy.x (x y)")] // A bit of everything
    [TestCase("λx.λy.λz.λa.λb.λc.x y z a b c", "λx.x")] // A bit of everything
    public void EtaReduction_RunsSuccessfully_AndReturnsInSimplifiedForm(string expression, string simplified)
    {
        var lambda = ExpressionParser.ParseExpression(expression)!
            .Simplify()
            .EtaReduction();
        Assert.That(lambda.ToString(), Is.EqualTo(simplified));
        Assert.True(lambda.IsWellFormatted());
    }
}