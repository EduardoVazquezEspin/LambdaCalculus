using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class AddToHistoryHandler : OnSubmitHandlerNodeSync
{
    public AddToHistoryHandler(ManagerInjector managerInjector) : base(managerInjector) { }

    public override HandlerResult HandleOnSubmit(string input)
    {
        ManagerInjector.LambdaManager.InputHistory.Add(input);
        return new HandlerResult(true, new List<string>(), Flow.Continue);
    }
}