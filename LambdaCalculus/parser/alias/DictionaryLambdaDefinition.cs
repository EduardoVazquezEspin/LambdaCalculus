using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

internal class DictionaryLambdaDefinition : ILambdaDefinition
{
    private readonly Dictionary<string, Expression> _expressionDictionary;
    private readonly Dictionary<string, string> _aliasDictionary;

    public DictionaryLambdaDefinition() { 
        _expressionDictionary = new Dictionary<string, Expression>();
        _aliasDictionary = new Dictionary<string, string>();
    }
    
    public bool TryAddToContext(string name, Expression expression)
    {
        var result = _expressionDictionary.TryAdd(name, expression);
        _aliasDictionary.TryAdd(expression.GetHashCode(), name);
        return result;
    }
    
    public bool TryGetExpression(string name, [NotNullWhen(true)] out Expression? expression)
    {
        return _expressionDictionary.TryGetValue(name, out expression);
    }

    public bool TryGetAlias(Expression expression, [NotNullWhen(true)] out string alias)
    {
        return _aliasDictionary.TryGetValue(expression.GetHashCode(), out alias!);
    }
}