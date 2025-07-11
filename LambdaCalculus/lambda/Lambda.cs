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
        var result = Expression.Parent == this && Expression.IsWellFormatted();
        if (!result)
            throw new Exception("Not well formatted");
        return result;
    }

    internal override int GetContextSize()
    {
        return base.GetContextSize() + 1;
    }   
    
    protected override Definition? GetLocalVariable(uint id)
    {
        if (Definition.PreId == id || Definition.Id == id)
            return Definition;
        return base.GetLocalVariable(id);
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

    public override Lambda Copy()
    {
        var definition = Definition.Copy();
        var newLambda = new Lambda(definition, Expression, ParenthesisType) {Parent = Parent};
        var expression = Expression.Copy();
        Expression.Parent = this;
        expression.Parent = newLambda;
        newLambda.Expression = expression;
        return newLambda;
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath)
    {
        Expression.GetAllBetaReductionOptionsRecursive(list, height +1, right, currentPath);
    }

    public override Expression BetaReduction(BetaReductionOption option)
    {
        Expression = Expression.BetaReduction(option);
        return this;
    }

    internal override Expression Substitute(Definition definition, Expression expression)
    {
        Expression = Expression.Substitute(definition, expression);
        return this;
    }

    internal override void RemoveVariableCalls()
    {
        Expression.RemoveVariableCalls();        
    }
}