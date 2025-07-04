namespace LambdaCalculus.lambda;

public enum ParseOptions{
    ParseExpression,
    ParseLambda
}

public sealed class ExpressionParser
{
    private string _expressionStr;
    public string ExpressionStr => _expressionStr;

    private int _index;
    private bool _hasError;
    private Expression? _lastExpression;
    private AbstractExpressionBuilder? _builder;
    private List<AbstractExpressionBuilder> _queue = null!;
    
    private Dictionary<string, Variable> _globalContext = null!;
    public Dictionary<string, Variable> GlobalContext => _globalContext;
    
    public Expression Parse(string expressionStr, ParseOptions options = ParseOptions.ParseExpression)
    {
        _expressionStr = expressionStr + '\n';
        _index = 0;
        _hasError = false;
        _lastExpression = null;
        _globalContext = new Dictionary<string, Variable>();

        _queue = new List<AbstractExpressionBuilder>();
        _builder = options == ParseOptions.ParseExpression
            ? new GenericExpressionBuilder(_globalContext)
            : new LambdaBuilder(_globalContext);
        _queue.Add(_builder);

        while (!_hasError && _builder != null && _index < _expressionStr.Length)
        {
            Analyze(_expressionStr[_index]);
        }

        if (_hasError)
        {
            if (_index == _expressionStr.Length - 1)
                throw new Exception("Something went wrong while parsing the expression");
            
            var first = _expressionStr.Substring(0, _index);
            var c = _expressionStr[_index];
            var second = _expressionStr.Substring(_index + 1, _expressionStr.Length - _index - 2);
            throw new Exception($"Invalid character found in position {_index}:\n{first}<b>{c}</b>{second}");
        }

        if (_queue.Any())
        {
            
            throw new Exception("Unfinished expression");
        }
        
        if (_lastExpression == null)
            throw new Exception("Empty expression");

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
                var expression = _builder.Build();
                _queue.RemoveAt(_queue.Count - 1);
                _builder = _queue.Count != 0 ? _queue[^1] : null;
                if (expression != null)
                {
                    _lastExpression = expression;
                    _builder?.BackToYou(expression);
                }
                break;
            case Flow.ParseExpression:
                _builder = new GenericExpressionBuilder(_globalContext, _builder);
                _queue.Add(_builder);
                break;
            case Flow.ParseLambda:
                _builder = new LambdaBuilder(_globalContext, _builder);
                _queue.Add(_builder);
                break;
            case Flow.ParseParenthesis:
                _builder = new ParenthesisBuilder(_globalContext, _builder);
                _queue.Add(_builder);
                break;
            case Flow.ParseVariable:
                _builder = new OldVariableBuilder(_globalContext, _builder);
                _queue.Add(_builder);
                break;
            case Flow.ParseNewVariable:
                _builder = new NewVariableBuilder(_globalContext, _builder);
                _queue.Add(_builder);
                break;
            case Flow.Error:
                _hasError = true;
                return;
        }
    }
}