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

    public virtual bool IsWellFormatted()
    {
        return true;
    }

    protected virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }

    public abstract Expression EtaReduction();

    public List<BetaReductionOption> GetAllBetaReductionOptions()
    {
        var list = new List<BetaReductionOption>();
        GetAllBetaReductionOptionsRecursive(list, 0, 0);
        return list;
    }

    internal abstract void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right);
}