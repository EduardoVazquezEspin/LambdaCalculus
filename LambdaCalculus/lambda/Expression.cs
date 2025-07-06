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
    public Expression EtaReduction()
    {
        var hashSet = new HashSet<string>();
        Expression current = this;
        string currentHashcode = current.GetHashCode();
        do
        {
            hashSet.Add(currentHashcode);
            current = current.EtaReductionRecursive();
            current = current.Simplify();
            currentHashcode = current.GetHashCode();
        } while (!hashSet.Contains(currentHashcode));

        return current;
    }

    internal abstract Expression EtaReductionRecursive();

    public virtual bool IsWellFormatted()
    {
        return true;
    }

    protected virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }
}