using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeSpecialCharactersHandler : OnTypeHandlerNodeSync
{
    public OnTypeSpecialCharactersHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override HandlerResult HandleOnType(ConsoleKeyInfo info)
    {
        switch (info.Key)
        {
            case ConsoleKey.Backspace:
                ManagerInjector.InputManager.CursorController.Delete(1);
                return new HandlerResult(false, new List<string>(), Flow.StopPropagation);
            default:
                return new HandlerResult(false, new List<string>(), Flow.Continue);
        }
    }
}