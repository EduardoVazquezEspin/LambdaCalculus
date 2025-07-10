//using LambdaCalculus.lambda;
//
//namespace LambdaCalculusTests.lambda;
//
//public class LambdaComputeDevelopmentTests
//{
//    [TestCase("[λf.f f] (λx.λy.x) λx.λy.x", "λx.λy.x")]
//    [TestCase("[λf.f f] (λx.λy.x) λx.λy.y", "λx.λy.x")]
//    [TestCase("[λf.f f] (λx.λy.y) λx.λy.x", "λx.λy.x")]
//    [TestCase("[λf.f f] (λx.λy.y) λx.λy.y", "λx.λy.y")]
//    [TestCase("[λf.f f] λf.f f", "[λf.f f] λf.f f")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.x) λx.λy.x", "λx.λy.x")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.x) λx.λy.y", "λx.λy.y")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.y) λx.λy.x", "λx.λy.y")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.y) λx.λy.y", "λx.λy.y")]
//    public void LambdaCompute_RunsSuccessfully_AndReturnsTheCorrectResult(string expressionStr, string resultStr)
//    {
//        var expression = ExpressionParser.ParseExpression(expressionStr)!;
//        var result = expression.Compute();
//        result.IsWellFormatted();
//        Assert.That(result.ToString(), Is.EqualTo(resultStr));
//    }
//    
//    [TestCase("[λf.f f] (λx.λy.x) λx.λy.x", "λa.λb.a")]
//    [TestCase("[λf.f f] (λx.λy.x) λx.λy.y", "λa.λb.a")]
//    [TestCase("[λf.f f] (λx.λy.y) λx.λy.x", "λa.λb.a")]
//    [TestCase("[λf.f f] (λx.λy.y) λx.λy.y", "λa.λb.b")]
//    [TestCase("[λf.f f] λf.f f", "[λx.x x] λy.y y")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.x) λx.λy.x", "λn.λm.n")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.x) λx.λy.y", "λn.λm.m")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.y) λx.λy.x", "λn.λm.m")]
//    [TestCase("(λb1.λb2.b1 b2 λx.λy.y) (λx.λy.y) λx.λy.y", "λn.λm.m")]
//    public void LambdaCompute_RunsSuccessfully_AndReturnsEquivalentResult(string expressionStr, string resultStr)
//    {
//        var expression = ExpressionParser.ParseExpression(expressionStr)!;
//        var result = expression.Compute();;
//        result.IsWellFormatted();
//        var expected = ExpressionParser.ParseExpression(resultStr);
//        Assert.That(result.ToString(), Is.Not.EqualTo(resultStr));
//        Assert.True(result.Equals(expected));
//    }
//
//    [TestCase("λy.y (λx.y y y x) λx.λy.x", "λy.y (y y y) λx.λy.x")]
//    [TestCase("λx.x λy.λx.y y y x", "λx.x λy.y y y")]
//    [TestCase("λy.(λx.y y y x) λx.λy.x", "λy.y y y λx.λy.x")]
//    [TestCase("λx.(λy.y y) (λy.y y) x", "(λy.y y) λy.y y")]
//    [TestCase("λx.λy.x y λz.x y z", "λx.λy.x y (x y)")]
//    [TestCase("λx.x λy.x y", "λx.x x")]
//    [TestCase("λx.(λy.x y) x", "λx.x x")]
//    [TestCase("λx.λy.(λx.x x) y", "λx.λx.x x")]
//    [TestCase("λx.x λy.(λx.x x) y", "λx.x λx.x x")]
//    [TestCase("λx.x (λy.(λx.x x) y) x", "λx.x (λx.x x) x")]
//    [TestCase("λa.λx.a λy.x y", "λa.a")]
//    [TestCase("λa.(λb.a λx.(λy.b y) x)", "λa.a")]
//    [TestCase("λx.(λx.x) x", "λx.x")] 
//    [TestCase("λx.λy.λz.(x y) z", "λx.x")]
//    [TestCase("λx.λy.x λz.x y z", "λx.λy.x (x y)")] 
//    [TestCase("λx.λy.λz.λa.λb.λc.x y z a b c", "λx.x")] 
//    [TestCase("(λn.λm.λf.λx.n f (m f x)) (λf.λx.f (f x)) λf.λx.f (f (f x))", "λf.λx.f (f (f (f (f x))))")] // 2 + 3 = 5
//    [TestCase("(λm.λn.λf.m (n f)) (λf.λx.f (f x)) λf.λx.f (f (f x))", "λf.λx.f (f (f (f (f (f x)))))")] // 2 * 3 = 6
//    [TestCase("(λf.λx.f (f (f x))) (λn.λf.λx.f (n f x)) λf.λx.x", "λf.λx.f (f (f x))")] // 3 ++ 0 = 3
//    [TestCase("(λf.λy.f (f (f y))) (λn.λf.λx.f (n f x)) λf.λx.x", "λf.λx.f (f (f x))")] // 3 ++ 0 = 3
//    [TestCase("(λn.λf.λx.f (n f x)) λf.λx.x", "λf.λx.f x")] // ++ 0 = 1
//    [TestCase("(λn.λf.λx.f (n f x))((λn.λf.λx.f (n f x)) λf.λx.x)", "λf.λx.f (f x)")] // ++ (++ 0) = 2
//    [TestCase("(λn.λf.λx.f (n f x))((λn.λf.λx.f (n f x))((λn.λf.λx.f (n f x)) λf.λx.x))", "λf.λx.f(f (f x))")] // ++ (++ (++ 0)) = 3
//    public void LambdaCompute_RunsSuccessfully_GenericListOfPositiveCases(string expressionStr, string resultStr)
//    {
//        var expression = ExpressionParser.ParseExpression(expressionStr)!;
//        var result = expression.Compute();
//        result.IsWellFormatted();
//        var expected = ExpressionParser.ParseExpression(resultStr);
//        Assert.True(result.Equals(expected));
//    }
//}