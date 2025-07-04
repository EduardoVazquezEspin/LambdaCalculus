namespace LambdaCalculus.lambda;

internal class LambdaBuilder : AbstractExpressionBuilder
{
    private enum LambdaBuilderState
    {
        Lambda,
        Variable,
        Point,
        Expression,
        Finished
    }

    private LambdaBuilderState _state;
    private bool _hasError;
    private Variable? _variable;
    private Expression? _expression;

    public LambdaBuilder(
        Dictionary<string, Variable> globalContext,
        AbstractExpressionBuilder? parent = null
    ) : base(globalContext, parent)
    {
        _state = LambdaBuilderState.Lambda;
        _hasError = false;
    }

    public override Flow Analyze(char c)
    {
        if (c == ' ')
            return Flow.Continue;
        if (_hasError)
            return Flow.Error;
        switch (_state)
        {
            case LambdaBuilderState.Lambda:
                return AnalyzeLambda(c);
            case LambdaBuilderState.Variable:
                return AnalyzeVariable(c);
            case LambdaBuilderState.Point:
                return AnalyzePoint(c);
            case LambdaBuilderState.Expression:
                return AnalyzeExpression(c);
            case LambdaBuilderState.Finished: default: 
                return AnalyzeFinished(c);
        }
    }

    private Flow AnalyzeLambda(char c)
    {
        if (c != 'λ')
            return Flow.Error;

        _state = LambdaBuilderState.Variable;
        return Flow.Continue;
    }

    private Flow AnalyzeVariable(char c)
    {
        return Flow.ParseNewVariable;
    }

    private Flow AnalyzePoint(char c)
    {
        if (c != '.')
            return Flow.Error;
        
        _state = LambdaBuilderState.Expression;
        return Flow.Continue;
    }

    private Flow AnalyzeExpression(char c)
    {
        return Flow.ParseExpression;
    }

    private Flow AnalyzeFinished(char c)
    {
        return Flow.Build;
    }

    public override void BackToYou(Expression lastParsedExpression)
    {
        switch (_state)
        {
            case LambdaBuilderState.Variable:
                _state = LambdaBuilderState.Point;
                if (!(lastParsedExpression is Variable variable))
                {
                    _hasError = true;
                    return;
                }
                _variable = variable;
                return;
            case LambdaBuilderState.Expression:
                _state = LambdaBuilderState.Finished;
                _expression = lastParsedExpression;
                return;
        }
    }

    public override Expression? Build(out ParseError? error)
    {
        error = null;
        if (_variable == null || _expression == null)
        {
            error = new SomethingWentWrong();
            return null;
        }
        var lambda = new Lambda(_variable, _expression, GlobalContext);
        return lambda;
    }

    protected override Variable? GetLocalVariable(string name)
    {
        if (_variable?.Name == name)
            return _variable;
        
        return base.GetLocalVariable(name);
    }
}