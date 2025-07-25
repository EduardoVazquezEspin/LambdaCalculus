using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

public sealed class LambdaParser
{
    private enum ParseOptions{
        ParseExpression,
        ParseLambda
    }
    
    private string _expressionStr = null!;

    private int _index;
    private bool _hasError;
    private ParseError? _error;
    private Expression? _lastExpression;
    private AbstractExpressionBuilder? _builder;
    private List<AbstractExpressionBuilder> _queue = null!;
    private readonly AliasManager _aliasManager;

    public LambdaParser()
    {
        _aliasManager = new AliasManager();
    }
    
    private Expression? Parse(string expressionStr, out ParseError error, ParseOptions options)
    {
        _expressionStr = expressionStr + '\n';
        _index = 0;
        _hasError = false;
        _error = null;
        _lastExpression = null;

        _queue = new List<AbstractExpressionBuilder>();
        _builder = options == ParseOptions.ParseExpression
            ? new GenericExpressionBuilder()
            : new LambdaBuilder();
        _queue.Add(_builder);

        while (!_hasError && _builder != null && _index < _expressionStr.Length)
        {
            Analyze(_expressionStr[_index]);
        }
        
        if (_error is not null)
        {
            error = _error;
            return null;    
        }

        if (_hasError)
        {
            error = new SomethingWentWrong();
            return null;
        }

        if (_queue.Any())
        {
            error = new UnfinishedExpression();
            return null;
        }

        if (_lastExpression == null)
        {
            error = new EmptyExpression();
            return null;
        }

        error = new NoError();
        return _lastExpression;
    }

    private void Analyze(char c)
    {
        var analysis = _builder!.Analyze(c);
        switch (analysis)
        {
            case Flow.Continue:
                _index++;
                return;
            case Flow.Build:
                var expression = _builder.Build(out var error);
                _queue.RemoveAt(_queue.Count - 1);
                _builder = _queue.Count != 0 ? _queue[^1] : null;
                if (error is not null && error is not NoError)
                {
                    _hasError = true;
                    _error = error is FreeVariableBuilder errorBuilder 
                        ? new FreeVariable(_expressionStr.Substring(0, _expressionStr.Length-1), _index - errorBuilder.Length, _index)
                        : error;
                    return;
                }

                if (expression is null)
                {
                    _hasError = true;
                    _error = new SomethingWentWrong();
                    return;
                }
                _lastExpression = expression;
                _builder?.BackToYou(expression);
                return;
            case Flow.ParseExpression:
                _builder = new GenericExpressionBuilder(_builder);
                _queue.Add(_builder);
                break;
            case Flow.ParseLambda:
                _builder = new LambdaBuilder(_builder);
                _queue.Add(_builder);
                return;
            case Flow.ParseParenthesis:
                _builder = new ParenthesisBuilder(_builder);
                _queue.Add(_builder);
                return;
            case Flow.ParseVariable:
                _builder = new OldVariableBuilder(_aliasManager, _builder);
                _queue.Add(_builder);
                return;
            case Flow.ParseNewVariable:
                _builder = new NewVariableBuilder(_builder);
                _queue.Add(_builder);
                return;
            case Flow.Error:
                _hasError = true;
                if (_index == _expressionStr.Length - 1)
                {
                    _error = new SomethingWentWrong();
                    return;
                }
                _error = new InvalidCharacter(_expressionStr.Substring(0, _expressionStr.Length-1), _index);
                return;
        }
    }

    public Expression? ParseExpression(string expression)
    {
        return Parse(expression, out ParseError _, ParseOptions.ParseExpression);
    }

    public Expression? ParseExpression(string expression, out ParseError error)
    {
        return Parse(expression, out error, ParseOptions.ParseExpression);
    }

    public Lambda? ParseLambda(string expression)
    {
        return Parse(expression, out ParseError _, ParseOptions.ParseLambda) as Lambda;
    }

    public Lambda? ParseLambda(string expression, out ParseError error)
    {
        return Parse(expression, out error, ParseOptions.ParseLambda) as Lambda;
    }

    public bool TryAddToContext(string name, Expression expression)
    {
        return _aliasManager.TryAddToContext(name, expression);
    }

    public bool TryGetAlias(Expression expression, [NotNullWhen(true)] out string? alias)
    {
        return _aliasManager.TryGetAlias(expression, out alias);
    }
}