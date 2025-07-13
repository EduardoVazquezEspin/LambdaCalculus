using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

public class DictionaryLambdaDefinition : ILambdaDefinition
{
    private readonly Dictionary<string, Expression> _expressionDictionary;
    private readonly Dictionary<string, string> _aliasDictionary;

    public DictionaryLambdaDefinition() { 
        _expressionDictionary = new Dictionary<string, Expression>();
        _aliasDictionary = new Dictionary<string, string>();
    }
    
    internal void AddToContext(string name, Expression expression)
    {
        _expressionDictionary.Add(name, expression);
        _aliasDictionary.TryAdd(expression.GetHashCode(), name);
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