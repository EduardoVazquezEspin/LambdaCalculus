using LambdaCalculusAppTests.mocks;

namespace LambdaCalculusAppTests.app.handlers;

public class EndExecutionHandlerE2ETests
{
    private MockConsoleApp _app = null!;
    private MockInputKey _pressEnter = null!;
    
    [SetUp]
    public void Setup()
    {
        _app = new MockConsoleApp();
        _pressEnter = new MockInputKey(ConsoleKey.Enter);
    }
    
    [Test]
    public async Task WritingExit_EndsTheProgram_AndSaysGoodbye()
    {
        var response = await _app.Run(new MockInputString("EXIT"), _pressEnter);
        Assert.True(response[^2].Contains("Bye"));
    }
}