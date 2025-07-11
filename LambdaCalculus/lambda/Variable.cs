namespace LambdaCalculus.lambda;

public class Variable : Expression
{
    public Definition Definition { get; }

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
        return GetLocalVariable(Definition.Id) == Definition;
    }
    
    public override Expression EtaReduction()
    {
        return this;
    }

    public override Variable Copy()
    {
        var definition = GetLocalVariable(Definition.Id);
        if (definition is null)
            throw new Exception("Something went wrong");
        definition.Calls++;
        return new Variable(definition);
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath) { }

    public override Expression BetaReduction(BetaReductionOption option)
    {
        return this;
    }

    internal override Expression Substitute(Definition definition, Expression expression)
    {
        if (definition != Definition)
            return this;

        var copy = expression.Copy();
        copy.Parent = Parent;
        return copy;
    }

    internal override void RemoveVariableCalls()
    {
        Definition.Calls--;
    }
}