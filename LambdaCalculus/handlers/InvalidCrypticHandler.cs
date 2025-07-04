using LambdaCalculus.managers;
using LambdaCalculus.managers.input;

namespace LambdaCalculus.handlers;

public sealed class InvalidCrypticHandler : HandlerNodeSync
{
    public override HandlerResult HandleOnSubmit(string input)
    {
        ManagerInjector.ViewManager.InvalidCrypticView();
        return new HandlerResult(false, new List<string>{"Invalid Cryptic"});
    }

    public InvalidCrypticHandler(ManagerInjector managerInjector) : base(managerInjector) { }
}