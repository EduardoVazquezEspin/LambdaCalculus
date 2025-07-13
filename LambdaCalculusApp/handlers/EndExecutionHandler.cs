using System.Text.RegularExpressions;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public sealed class EndExecutionHandler : OnSubmitHandlerNodeSync
{
    private readonly Regex _endCrypticRegex = new(@"^\s*(#|EXIT|END|FI|THE\s*END|FIN|E)\s*#?\s*$", RegexOptions.IgnoreCase);

    public override HandlerResult HandleOnSubmit(string input)
    {
        if (!_endCrypticRegex.Match(input).Success)
            return new HandlerResult(false, new List<string>() ,Flow.Continue);

        ManagerInjector.ViewManager.EndSessionView();
        return new HandlerResult(true, new List<string>(), Flow.EndExecution);
    }

    public EndExecutionHandler(ManagerInjector managerInjector) : base(managerInjector) { }
}