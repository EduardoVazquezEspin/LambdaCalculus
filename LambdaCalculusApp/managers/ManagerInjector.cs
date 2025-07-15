using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.managers;

public class ManagerInjector
{
    public InputManager InputManager { get; init; } = default!;
    public ViewManager ViewManager { get; init; } = default!;
    public LambdaManager LambdaManager { get; init; } = default!;
    public IFileSystemManager FileSystemManager { get; init; } = default!;
}