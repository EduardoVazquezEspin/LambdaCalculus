namespace LambdaCalculus.lambda;

public class Lambda : Expression, IParenthesisHolder
{
    public Variable Variable { get; private set; }
    public Expression Expression { get; private set; }
    public ParenthesisType ParenthesisType { get; set; }

    public Lambda(
        Variable variable,
        Expression expression,
        ParenthesisType parenthesisType = ParenthesisType.Round
    )
    {
        Variable = variable;
        Variable.Parent = this;
        Expression = expression;
        Expression.Parent = this;
        ParenthesisType = parenthesisType;
    }

    public override bool IsWellFormatted()
    {
        if (Variable.Parent != this || !Variable.IsWellFormatted())
            throw new Exception("Not well formatted");
        var result = Expression is Variable || Expression.Parent == this && Expression.IsWellFormatted();
        if (!result)
            throw new Exception("Not well formatted");
        return result;
    }

    protected override int GetContextSize()
    {
        return base.GetContextSize() + 1;
    }

    protected override Variable? GetLocalVariable(uint id)
    {
        if (Variable.PreId == id || Variable.Id == id)
            return Variable;
        return base.GetLocalVariable(id);
    }

    internal override Variable? GetLocalVariableByName(string name)
    {
        if (Variable.Name == name)
            return Variable;
        return base.GetLocalVariableByName(name);
    }

    public override Lambda Copy()
    {
        var originalVariable = Variable;
        Variable = new Variable(Variable.Name, 0, originalVariable.Id);
        var originalExpressionParent = Expression.Parent;
        var expression = CopyChild(Expression);
        var lambda = new Lambda(Variable, expression, ParenthesisType);
        Variable = originalVariable;
        if (Expression is Variable)
            Expression.Parent = originalExpressionParent;
        return lambda;
    }

    public override Expression EtaReduction()
    {
        Expression = Expression.EtaReduction();
        if (Variable.Calls != 1 || Expression is not Composition composition || composition.RightExpression != Variable)
            return this;
        if(composition.LeftExpression is not lambda.Variable)
            composition.LeftExpression.Parent = Parent;
        return composition.LeftExpression;
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath)
    {
        Expression.GetAllBetaReductionOptionsRecursive(list, height + 1, right, currentPath);
    }

    public override Expression BetaReduction(BetaReductionOption option)
    {
        Expression = Expression.BetaReduction(option);
        return this;
    }

    internal override void RemoveVariableCalls()
    {
        Expression.RemoveVariableCalls();
    }

    protected override Expression Substitute(Variable variable, Expression expression)
    {
        Expression = SubstituteChild(Expression, variable, expression);
        if(Expression is not lambda.Variable)
            Expression.Parent = this;
        return this;
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
        var body =$"λ{Variable.ToString(cache, taken)}.{Expression.ToString(cache, taken)}";
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
}