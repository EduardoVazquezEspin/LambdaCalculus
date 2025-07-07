namespace LambdaCalculus.lambda;

public class Variable : Expression
{
    public string Name { get; }
    public int Calls { get; internal set; }

    public Variable(string name, int calls)
    {
        Name = name;
        Calls = calls;
    }

    public override string ToString()
    {
        return Name;
    }

    public override string GetHashCode()
    {
        return GetContextSize().ToString();
    }

    public override Expression EtaReduction()
    {
        return this;
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right) { }
}