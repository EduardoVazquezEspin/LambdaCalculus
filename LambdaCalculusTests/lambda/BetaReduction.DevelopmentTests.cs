using LambdaCalculus;

namespace LambdaCalculusTests.lambda;

public class BetaReductionDevelopmentTests
{
    [TestCase("[λf.f f] {λx.λy.x} λx.λy.y", ExpectedResult = 1)]
    [TestCase("[λf.f f] ({λx.λy.x} λx.λy.y)", ExpectedResult = 2)]
    [TestCase("(λx.x) (λx.x) (λx.x) (λx.x) λx.x", ExpectedResult = 1)]
    [TestCase("(λx.x) (λx.x) (λx.x) [(λx.x) λx.x]", ExpectedResult = 2)]
    [TestCase("(λx.x) (λx.x) [(λx.x) λx.x] λx.x", ExpectedResult = 2)]
    [TestCase("(λx.x) (λx.x) [(λx.x) (λx.x) λx.x]", ExpectedResult = 2)]
    [TestCase("(λx.x) (λx.x) [(λx.x) [(λx.x) λx.x]]", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) λx.x] (λx.x) λx.x", ExpectedResult = 2)]
    [TestCase("(λx.x) [(λx.x) λx.x] [(λx.x) λx.x]", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) (λx.x) λx.x] λx.x", ExpectedResult = 2)]
    [TestCase("(λx.x) [(λx.x) [(λx.x) λx.x]] λx.x", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) (λx.x) (λx.x) λx.x]", ExpectedResult = 2)]
    [TestCase("(λx.x) [(λx.x) (λx.x) [(λx.x) λx.x]]", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) [(λx.x) λx.x] λx.x]", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) [(λx.x) (λx.x) λx.x]]", ExpectedResult = 3)]
    [TestCase("(λx.x) [(λx.x) [(λx.x) [(λx.x) λx.x]]]", ExpectedResult = 4)]
    public int GetAllBetaReductionOptions_IsSuccessful_AndReturnsAnIntegerValue(string expressionStr)
    {
        var expression = new LambdaParser().ParseExpression(expressionStr)!;
        expression.IsWellFormatted();
        return expression.GetAllBetaReductionOptions().Count;
    }
    
    [TestCase("[λf.f f] λx.x", "(λx.x) λx.x")]
    [TestCase("λx.{λf.f f} λy.y x", "λx.(λy.y x) λy.y x")]
    [TestCase("(λf.f f) λf.f f", "(λf.f f) λf.f f")]
    [TestCase("λz.λw.(λx.λy.y)w z", "λz.λw.(λy.y) z")]
    [TestCase("λz.λw.(λy.y) z", "λz.λw.z")]
    [TestCase("(λf.λy.f (f (f y))) (λn.λf.λx.f (n f x))", "λy.(λn.λf.λx.f (n f x)) ((λn.λf.λx.f (n f x)) ((λn.λf.λx.f (n f x)) y))")] // 3 ++
    public void BetaReduction_IsSuccessful_AndReturnsAValidExpression(string expressionStr, string expectedResult)
    {
        var expression = new LambdaParser().ParseExpression(expressionStr)!;
        expression.IsWellFormatted();
        var options = expression.GetAllBetaReductionOptions();
        var result = expression.BetaReduction(options[0]);
        Assert.True(result.IsWellFormatted());
        Assert.That(result.ToString(), Is.EqualTo(expectedResult));
    }
}