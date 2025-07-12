using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class RepeatCryptic : HandlerNodeSync
{
    public RepeatCryptic(ManagerInjector managerInjector) : base(managerInjector) { }

    public override HandlerResult HandleOnSubmit(string input)
    {
        Console.WriteLine(input);
        Console.Write("> ");
        return new HandlerResult(true, new List<string>());
    }
}