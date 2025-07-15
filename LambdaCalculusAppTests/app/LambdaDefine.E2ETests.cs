using LambdaCalculusApp.common;
using LambdaCalculusAppTests.mocks;

namespace LambdaCalculusAppTests.app;

public class LambdaDefineE2ETests
{
    private MockConsoleApp _app = null!;
    private MockInputKey _pressEnter = null!;
    private MockFileSystemManager _fileSystemManager = null!;
    
    [SetUp]
    public void Setup()
    {
        _app = new MockConsoleApp();
        _fileSystemManager = (MockFileSystemManager)_app.FileSystemManager;
        _pressEnter = new MockInputKey(ConsoleKey.Enter);
    }

    [TestCase("4", "2 → λ f  .λ x  .f (f x)", "4 → λ f.λ x  .f (f (f (f x)))", "+ → λ  first  .λ second .λf  .λx.   first   f (   second f x )", "   +  2  2")]
    public async Task LambdaDefine_SingleLineDefinition_PositiveCases(string output, params string[] input)
    {
        var inputs = input.SelectMany(it => new MockInput[] {new MockInputString(it), _pressEnter}).ToArray();
        var response = await _app.Run(inputs);
        Assert.True(response[^2].Contains(output));
    }

    [TestCase("+ 2 2", "4")]
    [TestCase("* 5 2", "10")]
    public async Task LambdaDefine_LoadFile_PositiveCases(string input, string output)
    {
        _fileSystemManager.ReadFileResponse = new Result<string[]>
        {
            Success = true,
            Messages = new List<string>(),
            Value = MockObjects.ArithmeticTxt 
        };
        
        var response = await _app.Run(new MockInputString("LOAD ARITHMETIC.txt"), _pressEnter, new MockInputString(input), _pressEnter);
        Assert.True(response[^2].Contains(output));
    }
    
    [TestCase("myfunc →", MockObjects.EmptyExpressionErrorMessage)]
    [TestCase("myfunc → λx.", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("myfunc → ([])", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("myfunc → ([)]", MockObjects.UnfinishedExpressionErrorMessage)]
    [TestCase("myfunc → λλx.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → λxλ.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → λ.x.x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → λx..x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → λx.λy.(x]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → ([λx.x)]", MockObjects.InvalidCharacterErrorMessage)]
    [TestCase("myfunc → λx.(x", MockObjects.SomethingWentWrongErrorMessage)]
    [TestCase("myfunc → good morning", MockObjects.FreeVariableErrorMessage)] 
    [TestCase("myfunc → λx.x myvariable myothervariable", MockObjects.FreeVariableErrorMessage)]
    public async Task LambdaDefine_SingleLineDefinition_InvalidDefinition(string input, string errorMessage)
    {
        var response = await _app.Run(new MockInputString(input), _pressEnter);
        Assert.True(response.Any(line => line.Contains(errorMessage)));
    }
    
    [Test]
    public async Task LambdaDefine_SingleLineDefinition_DoubleDefinition()
    {
        var response = await _app.Run(
            new MockInputString("ID → λx.x"), 
            _pressEnter,
            new MockInputString("ID → λx.x"), 
            _pressEnter
            );
        Assert.True(response.Any(line => line.Contains(MockObjects.DoubleDefinitionErrorMEssage)));
    }
}