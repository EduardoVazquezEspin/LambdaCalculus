namespace LambdaCalculus.lambda;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }
    public new abstract string ToString();
    public new abstract string GetHashCode();

    public new bool Equals(object? obj)
    {
        return obj is Expression objExpression && Equals(objExpression.GetHashCode(), GetHashCode());
    }

    public abstract Expression Simplify();

    public virtual bool IsWellFormatted()
    {
        return true;
    }

    public virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }
}