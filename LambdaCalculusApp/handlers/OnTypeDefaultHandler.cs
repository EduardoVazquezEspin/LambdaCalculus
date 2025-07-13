using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeDefaultHandler : OnTypeHandlerNodeSync
{
    public OnTypeDefaultHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override HandlerResult HandleOnType(ConsoleKeyInfo info)
    {
        ManagerInjector.InputManager.CursorController.Write(info.KeyChar);

        return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
    }
}