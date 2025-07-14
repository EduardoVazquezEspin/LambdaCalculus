using System.Text;
using LambdaCalculusApp;
using LambdaCalculusApp.managers;
using Moq;

namespace LambdaCalculusAppTests.mocks;

public class MockConsoleApp : ConsoleApp
{
    private readonly StringBuilder _consoleOutput;
    private readonly Mock<TextReader> _consoleInput;

    public IFileSystemManager FileSystemManager;
    
    public MockConsoleApp() : base()
    {
        Environment.SetEnvironmentVariable("TESTING", "TRUE");
        
        _consoleInput = new Mock<TextReader>();
        _consoleOutput = new StringBuilder();
        var consoleOutputWriter = new StringWriter(_consoleOutput);
        Console.SetOut(consoleOutputWriter);
        Console.SetIn(_consoleInput.Object);
    }
    
    protected override ConsoleApp ConfigureManagers()
    {
        FileSystemManager = new MockFileSystemManager();
        
        ManagerInjector = new ManagerInjector
        {
            InputManager = new MockInputManager(),
            ViewManager = new ViewManager(),
            LambdaManager = new LambdaManager(),
            FileSystemManager = FileSystemManager
        };

        return this;
    }

    public override Task Run()
    {
        throw new IllegalConsoleUsageDuringTesting();
    }
    
    public async Task<string[]> Run(params MockInput[] input)
    {
        await (ManagerInjector.InputManager as MockInputManager)!.Run(input);
        return _consoleOutput.ToString().Split("\n");
    }
}