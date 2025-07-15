using System.Text.RegularExpressions;
using LambdaCalculus;
using LambdaCalculusApp.common;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp.handlers;

public class LoadFileHandler : OnSubmitHandlerNodeSync
{
    public LoadFileHandler(ManagerInjector managerInjector) : base(managerInjector) { }

    private readonly Regex _loadFileRegex = new (@"^\s*LOAD\s*(?<filename>\.?[A-Z]+(\.[A-Z]+)*)\s*$", RegexOptions.IgnoreCase);
    private readonly Regex _isTextFileRegex = new (@"\.txt$");
    private readonly Regex _definitionRegex = new Regex(@"^\s*(?<name>[^\s]+)\s*\u2192\s*(?<lambda>.+)\s*$");
    
    public override HandlerResult HandleOnSubmit(string input)
    {
        var match = _loadFileRegex.Match(input);
        if (!match.Success)
            return new HandlerResult(false, new List<string>(), Flow.Continue);

        var fileName = match.Groups["filename"].Value;
        var isTextFile = _isTextFileRegex.Match(fileName).Success;

        if (!isTextFile)
        {
            var message = "File extension is not supported";
            ManagerInjector.ViewManager.Write(ConsoleColor.Red, message);
            return new HandlerResult(false, new List<string> {message}, Flow.StopPropagation);
        }

        var result = ManagerInjector.FileSystemManager.ReadFile(fileName);
        if (!result.Success)
        {
            ManagerInjector.ViewManager.Write(ConsoleColor.Red, result.Messages.ToArray());
            return new HandlerResult(false, result.Messages, Flow.StopPropagation);
        }

        var text = result.Value;
        var results = text.Select((line, index) => DefineGlobalVariable(index, line, fileName)).ToList();

        var errors = results.Where(it => !it.Success).SelectMany(it => it.Messages).ToList();

        var definitions = results
            .Where(it => it.Success)
            .Select(it => it.Value)
            .ToList();

        if(!definitions.Any())
            errors.Add($"No valid definitions found in file {fileName}");
        
        if (errors.Any())
            ManagerInjector.ViewManager.Write(ConsoleColor.Red, errors.ToArray());

        if (definitions.Any())
            ManagerInjector.ViewManager.Write(ConsoleColor.Green, $"Successfully loaded {definitions.Count} definitions from file {fileName}");

        return new HandlerResult(true, new List<string>(), Flow.StopPropagation);
    }

    private Result<KeyValuePair<string, Expression>> DefineGlobalVariable(int lineIndex, string textLine, string fileName)
    {
        var match = _definitionRegex.Match(textLine);
        if (!match.Success)
            return new Result<KeyValuePair<string, Expression>>
            {
                Success = false,
                Messages = new List<string>
                    {$"Error while loading file {fileName}: Line {lineIndex + 1} is not a lambda definition"}
            };

        var name = match.Groups["name"].Value;
        var lambda = match.Groups["lambda"].Value;

        var expression = ManagerInjector.LambdaManager.Parser.ParseExpression(lambda, out var error)!;
        if (error is not NoError)
            return new Result<KeyValuePair<string, Expression>>
            {
                Success = false,
                Messages = new List<string> {$"Error while loading file {fileName}: {error.Message}"}
            };
        
        var result = ManagerInjector.LambdaManager.Parser.TryAddToContext(name, expression);

        if (!result)
            return new Result<KeyValuePair<string, Expression>>
            {
                Success = false,
                Messages = new List<string> {$"There is already an expression with name {name}"}
            };
        
        return new Result<KeyValuePair<string, Expression>>
        {
            Success = true,
            Messages = new List<string>(),
            Value = new KeyValuePair<string, Expression>(name, expression)
        };
    }
}