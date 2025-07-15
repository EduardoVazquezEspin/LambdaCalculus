using LambdaCalculusAppTests.mocks;

namespace LambdaCalculusAppTests.app.handlers;

public class OnTypeArrowHandlerE2ETests
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
    public async Task TypeLeftArrow_AllowsToCorrectMistake()
    {
        var response = await _app.Run(
            new MockInputString("(λa.λb.b) (λc.λd.c) λx.λy.x"), 
            new MockInputKey(ConsoleKey.LeftArrow), 
            new MockInputString("y"), 
            _pressEnter
        );
        Assert.True(response[^2].Contains("λx.λy.y"));
    }

    [Test]
    public async Task TypeRightArrow_UndoesTypeLeftArrow()
    {
        var response = await _app.Run(
            new MockInputString("(λa.λb.b) (λc.λd.c) λx.λy.x"), 
            new MockInputKey(ConsoleKey.LeftArrow), 
            new MockInputKey(ConsoleKey.RightArrow),
            new MockInputKey(ConsoleKey.RightArrow),  
            new MockInputString("x"), 
            _pressEnter
        );
        Assert.True(response[^2].Contains("λx.λy.x x"));
    }
    
    [Test]
    public async Task TypeUpArrow_RecoversPreviousEntry()
    {
        var response = await _app.Run(
            new MockInputString("(λa.λb.b) (λc.λd.c) λx.λy.x"), 
            _pressEnter,
            new MockInputString("(λn.λm.λf.λx.n f (m f x)) (λf.λx.f (f x)) λf.λx.f (f (f x))"), 
            new MockInputKey(ConsoleKey.UpArrow),
            _pressEnter
        );
        Assert.True(response[^2].Contains("λx.λy.x"));
    }
}