using LambdaCalculusAppTests.mocks;

namespace LambdaCalculusAppTests.app.handlers;

public class InvalidCrypticHandlerE2ETests
{
    private MockConsoleApp _app = null!;
    private MockInputKey _pressEnter = null!;
    
    [SetUp]
    public void Setup()
    {
        _app = new MockConsoleApp();
        _pressEnter = new MockInputKey(ConsoleKey.Enter);
    }
    
    [TestCase("", MockObjects.EmptyExpressionErrorMessage)]
    [TestCase("λx.", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("([])", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("([)]", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("λλx.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("λxλ.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("λ.x.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("λx..x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("λx.λy.(x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("([λx.x)]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("λx.(x", MockObjects.SomethingWentWrongErrorMessage)]
    [TestCase("good morning", MockObjects.FreeVariableErrorMessage)] 
    [TestCase("λx.x myvariable myothervariable", MockObjects.FreeVariableErrorMessage)]
    public async Task WritingExit_EndsTheProgram_AndSaysGoodbye(string input, string errorMessage)
    {
        var response = await _app.Run(new MockInputString(input), _pressEnter);
        Assert.True(response.Any(line => line.Contains(errorMessage)));
    }
}