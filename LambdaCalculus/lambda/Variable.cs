namespace LambdaCalculus.lambda;

public class Variable : Expression
{
    internal Definition Definition { get; }

    public Variable(Definition definition)
    {
        Definition = definition;
    }

    internal override string ToString(Dictionary<uint, string> cache, HashSet<string> taken)
    {
        return Definition.ToString(cache, taken);
    }

    public override string GetHashCode()
    {
        return Definition.GetContextSize().ToString();
    }

    public override bool IsWellFormatted()
    {
        return true;
    }
    
    public override Expression EtaReduction()
    {
        return this;
    }
}