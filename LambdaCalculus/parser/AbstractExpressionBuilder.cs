namespace LambdaCalculus;

internal abstract class AbstractExpressionBuilder
{
    protected readonly AbstractExpressionBuilder? Parent;

    protected AbstractExpressionBuilder(
        AbstractExpressionBuilder? parent = null
        )
    {
        Parent = parent;
    }
    public abstract Flow Analyze(char c);

    public abstract void BackToYou(Expression lastParsedExpression);
    public abstract Expression? Build(out ParseError? error);

    protected virtual Definition? GetLocalVariable(string name)
    {
        return Parent?.GetLocalVariable(name);
    }
}