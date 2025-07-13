using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.managers;

public class ManagerInjector
{
    public InputManager InputManager { get; init; }
    public ViewManager ViewManager { get; init; }
    public LambdaManager LambdaManager { get; init; }
    public IFileSystemManager FileSystemManager { get; init; }
}