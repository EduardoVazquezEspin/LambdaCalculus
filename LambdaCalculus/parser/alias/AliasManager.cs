using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

public class AliasManager
{
    private DictionaryLambdaDefinition _defaultLambdaDefinition;
    private List<ILambdaDefinition> _lambdaDefinitions;
    internal AliasManager()
    {
        _defaultLambdaDefinition = new DictionaryLambdaDefinition();
        _lambdaDefinitions = new List<ILambdaDefinition>();
    }

    internal void AddToContext(string name, Expression expression)
    {
        _defaultLambdaDefinition.AddToContext(name, expression);
    }

    internal void AddLambdaDefinition(ILambdaDefinition lambdaDefinition)
    {
        _lambdaDefinitions.Add(lambdaDefinition);
    }

    internal void AddLambdaDictionary(List<KeyValuePair<string, Expression>> definitions)
    {
        var dictionaryLambdaDefinition = new DictionaryLambdaDefinition();
        foreach(var (name, expression) in definitions)
            dictionaryLambdaDefinition.AddToContext(name, expression);
        
        _lambdaDefinitions.Add(dictionaryLambdaDefinition);
    }

    internal bool TryGetExpression(string name, [NotNullWhen(true)] out Expression? expression)
    {
        foreach (var lambdaDefinition in _lambdaDefinitions)
            if (lambdaDefinition.TryGetExpression(name, out expression))
                return true;

        return _defaultLambdaDefinition.TryGetExpression(name, out expression);
    }

    internal bool TryGetAlias(Expression expression, [NotNullWhen(true)] out string alias)
    {
        foreach (var lambdaDefinition in _lambdaDefinitions)
            if (lambdaDefinition.TryGetAlias(expression, out alias))
                return true;
        
        return _defaultLambdaDefinition.TryGetAlias(expression, out alias);
    }
}