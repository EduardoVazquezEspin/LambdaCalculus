namespace LambdaCalculus.lambda;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }

    public sealed override string ToString()
    {
        var cache = new Dictionary<uint, string>();
        var taken = new HashSet<string>();
        return ToString(cache, taken);
    }
    internal abstract string ToString(Dictionary<uint, string> cache, HashSet<string> taken);
    public new abstract string GetHashCode();

    public new bool Equals(object? obj)
    {
        return obj is Expression objExpression && Equals(objExpression.GetHashCode(), GetHashCode());
    }

    public virtual bool IsWellFormatted()
    {
        return true;
    }

    internal virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }
    
    internal virtual Definition? GetLocalVariableByName(string name)
    {
        return Parent?.GetLocalVariableByName(name);
    }

    public abstract Expression EtaReduction();
}