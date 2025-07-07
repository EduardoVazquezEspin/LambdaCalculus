namespace LambdaCalculus.lambda;

public class Composition : Expression, IParenthesisHolder
{
    internal Expression LeftExpression { get; private set; }

    internal Expression RightExpression { get; private set; }

    public ParenthesisType ParenthesisType { get; set; }

    public Composition(Expression leftExpression, Expression rightExpression, ParenthesisType parenthesisType = ParenthesisType.Round)
    {
        LeftExpression = leftExpression;
        RightExpression = rightExpression;
        LeftExpression.Parent = this;
        RightExpression.Parent = this;
        ParenthesisType = parenthesisType;
    }
    
    public override bool IsWellFormatted()
    {
        if (LeftExpression is not Variable && (LeftExpression.Parent != this || !LeftExpression.IsWellFormatted()))
            return false;
        if (RightExpression is not Variable && (RightExpression.Parent != this || !RightExpression.IsWellFormatted()))
            return false;
        return true;
    }

    public override Expression EtaReduction()
    {
        LeftExpression = LeftExpression.EtaReduction();
        RightExpression = RightExpression.EtaReduction();
        return this;
    }

    public bool HasParenthesis()
    {
        if (Parent is not Composition composition)
            return false;
        return composition.RightExpression == this;
    }

    public override string ToString()
    {
        var body = $"{LeftExpression.ToString()} {RightExpression.ToString()}";
        if (!HasParenthesis())
            return body;
        return $"{ParenthesisType.GetOpenChar()}{body}{ParenthesisType.GetClosedChar()}";
    }
    
    public override string GetHashCode()
    {
        return $"({LeftExpression.GetHashCode()} {RightExpression.GetHashCode()})";
    }
}