using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeLHandler : OnTypeHandlerNodeSync
{
    public OnTypeLHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override HandlerResult HandleOnType(ConsoleKeyInfo info)
    {
        if (info.Key != ConsoleKey.L)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var cursor = ManagerInjector.InputManager.CursorController;
        if(cursor.Text.Length == 0)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var lastChar = cursor.LastChar();
        if (lastChar is 'L' or 'l')
        {
            cursor.Delete(1);
            cursor.Write('λ');
            return new HandlerResult(false, new List<string>(), Flow.StopPropagation);
        }
        
        if (lastChar == 'λ')
        {
            cursor.Delete(1);
            cursor.Write(info.KeyChar, info.KeyChar);
            return new HandlerResult(false, new List<string>(), Flow.StopPropagation);
        }

        return new HandlerResult(false, new List<string>(), Flow.Continue);
    }
}