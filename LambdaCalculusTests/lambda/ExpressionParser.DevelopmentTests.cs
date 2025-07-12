using LambdaCalculus;

namespace LambdaCalculusTests.lambda;

public class ExpressionParserDevelopmentTests
{
    [SetUp]
    public void Setup() { }
    
    [TestCase("λx.x")]
    [TestCase("λx.λy.x")]
    [TestCase("λx.λy.y")]
    [TestCase("λx.(λy.y) x")] // η reduction
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
    public void ExpressionParser_ParsesSuccessfully_AndStringifiesEqualToInput(string expression)
    {
        var lambda = ExpressionParser.ParseExpression(expression, out var error);
        Assert.That(error, Is.InstanceOf<NoError>());
        Assert.That(lambda!.ToString(), Is.EqualTo(expression));
        Assert.True(lambda.IsWellFormatted());
    }
    
    [TestCase("", typeof(EmptyExpression))]
    [TestCase("λx.",typeof(UnfinishedExpression))]
    [TestCase("([])",typeof(UnfinishedExpression))]
    [TestCase("([)]", typeof(UnfinishedExpression))]
    [TestCase("λλx.x]", typeof(InvalidCharacter))]
    [TestCase("λxλ.x]", typeof(InvalidCharacter))]
    [TestCase("λ.x.x]", typeof(InvalidCharacter))]
    [TestCase("λx..x]", typeof(InvalidCharacter))]
    [TestCase("λx.λy.(x]", typeof(InvalidCharacter))]
    [TestCase("([λx.x)]", typeof(InvalidCharacter))]
    [TestCase("à", typeof(InvalidCharacter))] 
    [TestCase("λx.(x",typeof(SomethingWentWrong))]
    [TestCase("λx.x myvariable myothervariable", typeof(FreeVariable))]
    public void ExpressionParser_ParsesUnsuccessfully_ReturnsNullAndError(string expression, Type type)
    {
        var lambda = ExpressionParser.ParseExpression(expression, out var error);
        Assert.That(error.GetType(), Is.EqualTo(type));
        Assert.Null(lambda);
    }
}