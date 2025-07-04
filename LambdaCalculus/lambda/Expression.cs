namespace LambdaCalculus.lambda;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }
    public new abstract string ToString();

    public abstract Expression Simplify();

    public virtual bool IsWellFormatted()
    {
        return true;
    }
}