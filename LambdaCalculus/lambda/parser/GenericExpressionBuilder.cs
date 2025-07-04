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
    public override Expression Build()
    {
        return new Composition(_expressions);
    }
}