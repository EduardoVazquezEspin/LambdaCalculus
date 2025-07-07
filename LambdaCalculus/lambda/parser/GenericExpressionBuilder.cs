namespace LambdaCalculus.lambda;

internal class GenericExpressionBuilder : AbstractExpressionBuilder
{
    private readonly List<Expression> _expressions;
    public GenericExpressionBuilder(
        Dictionary<string, Variable> globalContext,
        AbstractExpressionBuilder? parent = null
        ) : base(globalContext, parent)
    {
        _expressions = new List<Expression>();
    }

    public override Flow Analyze(char c)
    {
        if (c == ' ')
            return Flow.Continue;
        if (c == 'λ')
            return Flow.ParseLambda;
        if (Helpers.IsValidOpenParenthesis(c))
            return Flow.ParseParenthesis;
        if (Helpers.IsValidVariableChar(c))
            return Flow.ParseVariable;
        if (c == '\n' || Helpers.IsValidClosedParenthesis(c))
            return Flow.Build;
        return Flow.Error;
    }
    
    public override void BackToYou(Expression lastParsedExpression)
    {
        _expressions.Add(lastParsedExpression);
    }

    public override Expression? Build(out ParseError? error)
    {
        error = null;
        switch (_expressions.Count)
        {
            case 0:
                error = Parent is null ? new EmptyExpression() : new UnfinishedExpression();
                return null;
            case 1:
                return _expressions[0];
            default:
                var expression = new Composition(_expressions[0], _expressions[1]);
                for (int i = 2; i < _expressions.Count; i++)
                    expression = new Composition(expression, _expressions[i]);
                return expression;
        }
    }
}