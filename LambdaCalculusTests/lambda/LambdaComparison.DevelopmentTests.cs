using LambdaCalculus.lambda;

namespace LambdaCalculusTests.lambda;

public class LambdaComparisonDevelopmentTests
{
    [TestCase("λx.x", "λx.x", ExpectedResult = true)]
    [TestCase("λx.x", "λy.y", ExpectedResult = true)]
    [TestCase("(λx.x)", "λy.y", ExpectedResult = true)]
    [TestCase("λx.λy.x", "λx.λy.y", ExpectedResult = false)]
    [TestCase("λg.(λx.g(x x))(λx.g(x x))", "λA.([{  λB.  A [ B   B   ] }  ]  { [λZ.   A {   Z  Z   } ] })", ExpectedResult = true)]
    public bool LambdaComparison_SuccessfullyCompares_AndReturnsBoolean(string expressionStr1, string expressionStr2)
    {
        var expression1 = ExpressionParser.ParseExpression(expressionStr1)!;
        var expression2 = ExpressionParser.ParseExpression(expressionStr2)!;
        return expression1.Equals(expression2);
    }
}