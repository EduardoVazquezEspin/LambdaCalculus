using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public sealed class InvalidCrypticHandler : HandlerNodeSync
{
    public override HandlerResult HandleOnSubmit(string input)
    {
        ManagerInjector.ViewManager.InvalidCrypticView();
        return new HandlerResult(false, new List<string>{"Invalid Cryptic"}, Flow.StopPropagation);
    }

    public InvalidCrypticHandler(ManagerInjector managerInjector) : base(managerInjector) { }
}