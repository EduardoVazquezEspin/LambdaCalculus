using LambdaCalculusAppTests.mocks;

namespace LambdaCalculusAppTests.app;

public class LambdaComputeE2ETests
{
    private MockConsoleApp _app = null!;
    private MockInputKey _pressEnter = null!;
    
    [SetUp]
    public void Setup()
    {
        _app = new MockConsoleApp();
        _pressEnter = new MockInputKey(ConsoleKey.Enter);
    }

    [TestCase("(λx.x) λx.x x", "λx.x x")]    
    [TestCase("(λn.λm.λf.λx.n f (m f x)) (λf.λx.f (f x)) λf.λx.f (f (f x))", "λf.λx.f (f (f (f (f x))))")]
    public async Task LambdaCompute_PositiveCase(string input, string output)
    {
        var response = await _app.Run(new MockInputString(input), _pressEnter);
        Assert.True(response[^2].Contains(output));
    }
}