using LambdaCalculus.lambda;

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
        var expression = ExpressionParser.ParseExpression(expressionStr)!;
        expression.IsWellFormatted();
        return expression.GetAllBetaReductionOptions().Count;
    }
}