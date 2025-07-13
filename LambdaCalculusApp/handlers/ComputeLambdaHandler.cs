using System.Text.RegularExpressions;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class ComputeLambdaHandler : HandlerNodeSync
{
    public ComputeLambdaHandler(ManagerInjector managerInjector) : base(managerInjector) { }

    private Regex _lambdaRegex = new Regex(@"^\s*(?<lambda>.+)\s*$");
    
    public override HandlerResult HandleOnSubmit(string input)
    {
        var match = _lambdaRegex.Match(input);
        if (!match.Success)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var lambda = match.Groups["lambda"].Value;
        var expression = ManagerInjector.LambdaManager.Parser.ParseExpression(lambda, out var error);

        if (error is not LambdaCalculus.NoError)
        {
            ManagerInjector.ViewManager.Write(error.Message);
            return new HandlerResult(false, new List<string> {error.Message}, Flow.StopPropagation);
        }
            
        var result = expression!.Compute();
        string message;
        message = ManagerInjector.LambdaManager.Parser.TryGetAlias(result, out var alias) 
            ? alias 
            : result.ToString();
            
        
        ManagerInjector.ViewManager.Write(message);
        return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
    }
}