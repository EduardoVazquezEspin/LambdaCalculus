using System.Text.RegularExpressions;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public sealed class EndExecutionHandler : HandlerNodeSync
{
    private readonly Regex _endCrypticRegex = new(@"^\s*(#|EXIT|END|FI|THE\s*END|FIN|E)\s*#?\s*$");

    public override HandlerResult HandleOnSubmit(string input)
    {
        if (!_endCrypticRegex.Match(input.ToUpper()).Success)
            return new HandlerResult(Flow.Continue);

        ManagerInjector.ViewManager.EndSessionView();
        return new HandlerResult(true, new List<string>(), Flow.EndExecution);
    }

    public EndExecutionHandler(ManagerInjector managerInjector) : base(managerInjector) { }
}