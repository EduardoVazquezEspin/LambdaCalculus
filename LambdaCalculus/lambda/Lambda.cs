namespace LambdaCalculus.lambda;

public class Lambda : Expression, IParenthesisHolder
{
    public Definition Definition { get; private set; }
    public Expression Expression { get; private set; }
    public ParenthesisType ParenthesisType { get; set; }

    public Lambda(
        Definition definition,
        Expression expression,
        ParenthesisType parenthesisType = ParenthesisType.Round
    )
    {
        Definition = definition;
        Definition.Parent = this;
        Expression = expression;
        Expression.Parent = this;
        ParenthesisType = parenthesisType;
    }

    public override bool IsWellFormatted()
    {
        if (Definition.Parent != this || !Definition.IsWellFormatted())
            throw new Exception("Not well formatted");
        var result = Expression is Variable || Expression.Parent == this && Expression.IsWellFormatted();
        if (!result)
            throw new Exception("Not well formatted");
        return result;
    }

    internal override int GetContextSize()
    {
        return base.GetContextSize() + 1;
    }

    internal override Definition? GetLocalVariableByName(string name)
    {
        if (Definition.Name == name)
            return Definition;
        return base.GetLocalVariableByName(name);
    }

    private bool HasParenthesis()
    {
        if (Parent is not Composition composition)
            return false;
        if (composition.LeftExpression == this)
            return true;
        return composition.Parent is Composition && !composition.HasParenthesis();
    }

    internal override string ToString(Dictionary<uint, string> cache, HashSet<string> taken)
    {
        var body =$"λ{Definition.ToString(cache, taken)}.{Expression.ToString(cache, taken)}";
        if (!HasParenthesis())
            return body;
        return $"{ParenthesisType.GetOpenChar()}{body}{ParenthesisType.GetClosedChar()}"; 
    }
    
    public override string GetHashCode()
    {
        var body = $"λ{Expression.GetHashCode()}";
        if (!HasParenthesis())
            return body;
        return $"({body})";
    }
    
    public override Expression EtaReduction()
    {
        Expression = Expression.EtaReduction();
        if (
            Definition.Calls != 1 || 
            Expression is not Composition {RightExpression: Variable variable} composition || 
            variable.Definition != Definition
        )
            return this;
        composition.LeftExpression.Parent = Parent;
        return composition.LeftExpression;
    }
}