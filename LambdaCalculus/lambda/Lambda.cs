namespace LambdaCalculus.lambda;

public class Lambda : Expression, IParenthesisHolder
{
    public Variable Variable { get; }
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
        return Expression is Variable || Expression.Parent == this && Expression.IsWellFormatted();
    }

    protected override int GetContextSize()
    {
        return base.GetContextSize() + 1;
    }

    public override Expression EtaReduction()
    {
        Expression = Expression.EtaReduction();
        if (Variable.Calls != 1 || Expression is not Composition composition || composition.RightExpression != Variable)
            return this;
        composition.LeftExpression.Parent = Parent;
        return composition.LeftExpression;
    }

    private bool HasParenthesis()
    {
        if (Parent is not Composition composition)
            return false;
        if (composition.LeftExpression == this)
            return true;
        return composition.Parent is Composition && !composition.HasParenthesis();
    }

    public override string ToString()
    {
        var body =$"λ{Variable.ToString()}.{Expression.ToString()}";
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