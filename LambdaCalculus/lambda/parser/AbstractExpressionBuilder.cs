namespace LambdaCalculus.lambda;

internal abstract class AbstractExpressionBuilder
{
    private AbstractExpressionBuilder? _parent;
    
    protected readonly Dictionary<string, Variable> GlobalContext;

    protected AbstractExpressionBuilder(
        Dictionary<string, Variable> globalContext,
        AbstractExpressionBuilder? parent = null
        )
    {
        GlobalContext = globalContext;
        _parent = parent;
    }
    public abstract Flow Analyze(char c);

    public abstract void BackToYou(Expression lastParsedExpression);
    public abstract Expression? Build();

    protected virtual Variable? GetLocalVariable(string name)
    {
        return _parent?.GetLocalVariable(name);
    }
}