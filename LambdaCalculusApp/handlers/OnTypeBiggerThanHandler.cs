using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeBiggerThanHandler : OnTypeHandlerNodeSync
{
    public OnTypeBiggerThanHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override HandlerResult HandleOnType(ConsoleKeyInfo info)
    {
        if (info.KeyChar != '>')
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var cursor = ManagerInjector.InputManager.CursorController;
        if(cursor.Text.Length == 0)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var lastChar = cursor.LastChar();
        if(lastChar is not ('=' or '-'))
            return new HandlerResult(false, new List<string>(), Flow.Continue);
        
        cursor.Delete(1);
        cursor.Write('→');
        return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
    }
}