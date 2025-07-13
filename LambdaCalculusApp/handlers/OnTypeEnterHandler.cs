using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class OnTypeEnterHandler : OnTypeHandlerNodeAsync
{
    public OnTypeEnterHandler(ManagerInjector managerInjector) : base(managerInjector)
    {
    }

    public override async Task<HandlerResult> HandleOnType(ConsoleKeyInfo info)
    {
        if (info.Key != ConsoleKey.Enter)
            return new HandlerResult(false, new List<string>(), Flow.Continue);
        
        Console.WriteLine();
        var inputManager = ManagerInjector.InputManager;
        var result = await inputManager.HandleOnSubmit(inputManager.CursorController.Text);
        if(result.Flow != Flow.EndExecution)
            inputManager.CursorController.PressEnter();
        return result;
    }
}