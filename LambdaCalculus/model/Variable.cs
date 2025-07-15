namespace LambdaCalculus;

public class Variable : Expression
{
    public Definition Definition { get; }

    internal Variable(Definition definition)
    {
        Definition = definition;
    }

    internal override string ToString(Dictionary<uint, string> cache, HashSet<string> taken)
    {
        return Definition.ToString(cache, taken);
    }

    public override string GetHashCode()
    {
        return Definition.GetHashCode();
    }

    public override bool IsWellFormatted()
    {
        return GetLocalVariable(Definition.Id) == Definition;
    }

    public override Variable Copy()
    {
        var definition = GetLocalVariable(Definition.Id);
        if (definition is null)
            throw new Exception("Something went wrong");
        definition.Calls++;
        return new Variable(definition);
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