using LambdaCalculus;

namespace LambdaCalculusTests.lambda;

public class LambdaParserDevelopmentTests
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
    public void LambdaParser_ParsesSuccessfully_AndStringifiesEqualToInput(string expression)
    {
        var lambda = new LambdaParser().ParseExpression(expression, out var error);
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
    [TestCase("λx.(x",typeof(SomethingWentWrong))]
    [TestCase("à", typeof(FreeVariable))] 
    [TestCase("λx.x myvariable myothervariable", typeof(FreeVariable))]
    public void LambdaParser_ParsesUnsuccessfully_ReturnsNullAndError(string expression, Type type)
    {
        var lambda = new LambdaParser().ParseExpression(expression, out var error);
        Assert.That(error.GetType(), Is.EqualTo(type));
        Assert.Null(lambda);
    }

    [TestCase("λx.Id","Id=>λy.y", "λx.λy.y")]
    [TestCase("+ 3 2","+=>λn.λm.λf.λx.n f (m f x);3=>λf.λx.f (f (f x));2=>λf.λx.f (f x)", "(λn.λm.λf.λx.n f (m f x)) (λf.λx.f (f (f x))) λf.λx.f (f x)")]
    public void LambdaParser_TakesIntoAccountGlobalContext_AndReturnCorrectResponse(
        string expression, 
        string context,
        string expectedResult
        )
    {
        var parser = new LambdaParser();
        var parsedContext = context
            .Split(";")
            .Select(it => it.Split("=>"))
            .Select(pair => new KeyValuePair<string,Expression>(pair[0] ,parser.ParseExpression(pair[1])!));
        
        foreach (var (name, exp) in parsedContext)
            parser.TryAddToContext(name, exp);

        var result = parser.ParseExpression(expression)!;
        Assert.That(result.IsWellFormatted());
        Assert.That(result.ToString(), Is.EqualTo(expectedResult));
    }
}