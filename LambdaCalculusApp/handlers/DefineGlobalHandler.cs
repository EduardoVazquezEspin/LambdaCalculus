using System.Text.RegularExpressions;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class DefineGlobalHandler : HandlerNodeSync
{
    public DefineGlobalHandler(ManagerInjector managerInjector) : base(managerInjector) { }

    private Regex _lambdaRegex = new Regex(@"^\s*(?<name>[^\s]+)\s*\u2192\s*(?<lambda>.+)\s*$");
    
    public override HandlerResult HandleOnSubmit(string input)
    {
        var match = _lambdaRegex.Match(input);
        if (!match.Success)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var name = match.Groups["name"].Value;
        var lambda = match.Groups["lambda"].Value;
        var expression = ManagerInjector.LambdaManager.Parser.ParseExpression(lambda, out var error);

        if (error is not LambdaCalculus.NoError)
        {
            ManagerInjector.ViewManager.Write(error.Message);
            return new HandlerResult(false, new List<string> {error.Message}, Flow.StopPropagation);
        }

        ManagerInjector.LambdaManager.Parser.AddToContext(name, expression!);
        
        ManagerInjector.ViewManager.Write($"Successfully defined {name} as {expression!.ToString()} ");
        return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
    }
}