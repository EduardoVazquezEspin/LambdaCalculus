using LambdaCalculus.managers.input;

namespace LambdaCalculus.managers;

public class ManagerInjector
{
    public InputManager InputManager { get; init; }
    public ViewManager ViewManager { get; init; }
}