using LambdaCalculus.lambda;

namespace LambdaCalculusTests.lambda;

public class LambdaComparisonDevelopmentTests
{
    [TestCase("λx.x", "λx.x", false, ExpectedResult = true)]
    [TestCase("λx.x", "λx.x", true, ExpectedResult = true)]
    [TestCase("λx.x", "λy.y", false, ExpectedResult = true)]
    [TestCase("λx.x", "λy.y", true, ExpectedResult = true)]
    [TestCase("(λx.x)", "λy.y", false, ExpectedResult = false)]
    [TestCase("(λx.x)", "λy.y", true, ExpectedResult = true)]
    [TestCase("λg.(λx.g(x x))(λx.g(x x))", "λA.[{  λB.  A [ B   B   ] }  ]  { [λZ.   A {   Z  Z   } ] }", false, ExpectedResult = false)]
    [TestCase("λg.(λx.g(x x))(λx.g(x x))", "λA.[{  λB.  A [ B   B   ] }  ]  { [λZ.   A {   Z  Z   } ] }", true, ExpectedResult = true)]
    public bool LambdaComparison_SuccessfullyCompares_AndReturnsBoolean(string expressionStr1, string expressionStr2, bool simplify)
    {
        var expression1 = ExpressionParser.ParseExpression(expressionStr1)!;
        var expression2 = ExpressionParser.ParseExpression(expressionStr2)!;
        if (simplify)
        {
            expression1 = expression1.Simplify();
            expression2 = expression2.Simplify();
        }
        return expression1.Equals(expression2);
    }
}