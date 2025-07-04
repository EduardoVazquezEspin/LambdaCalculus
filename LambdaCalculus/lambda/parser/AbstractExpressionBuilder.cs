namespace LambdaCalculus.lambda;

internal abstract class AbstractExpressionBuilder
{
    protected readonly AbstractExpressionBuilder? Parent;
    
    protected readonly Dictionary<string, Variable> GlobalContext;

    protected AbstractExpressionBuilder(
        Dictionary<string, Variable> globalContext,
        AbstractExpressionBuilder? parent = null
        )
    {
        GlobalContext = globalContext;
        Parent = parent;
    }
    public abstract Flow Analyze(char c);

    public abstract void BackToYou(Expression lastParsedExpression);
    public abstract Expression? Build(out ParseError? error);

    protected virtual Variable? GetLocalVariable(string name)
    {
        return Parent?.GetLocalVariable(name);
    }
}