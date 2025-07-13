using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeArrowHandler : OnTypeHandlerNodeSync
{
    public OnTypeArrowHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override HandlerResult HandleOnType(ConsoleKeyInfo info)
    {
        var cursor = ManagerInjector.InputManager.CursorController;
        if (info.Key == ConsoleKey.RightArrow)
        {
            cursor.Move(1);
            return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
        }
        if (info.Key == ConsoleKey.LeftArrow)
        {
            cursor.Move(-1);
            return new HandlerResult(true, new List<string>(), Flow.StopPropagation);   
        }
        if (info.Key == ConsoleKey.UpArrow && ManagerInjector.LambdaManager.InputHistory.Any())
        {
            var lastEntry = ManagerInjector.LambdaManager.InputHistory[^1];
            cursor.WriteLine(lastEntry);
            return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
        }
            
        return new HandlerResult(false, new List<string>(), Flow.Continue);
    }
}