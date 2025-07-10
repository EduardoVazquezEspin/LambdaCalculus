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
        if (LeftExpression.Parent != this || !LeftExpression.IsWellFormatted())
            throw new Exception("Not well formatted");
        if (RightExpression.Parent != this || !RightExpression.IsWellFormatted())
            throw new Exception("Not well formatted");
        return true;
    }

    public bool HasParenthesis()
    {
        if (Parent is not Composition composition)
            return false;
        return composition.RightExpression == this;
    }

    internal override string ToString(Dictionary<uint, string> cache, HashSet<string> taken)
    {
        var body = $"{LeftExpression.ToString(cache, taken)} {RightExpression.ToString(cache, taken)}";
        if (!HasParenthesis())
            return body;
        return $"{ParenthesisType.GetOpenChar()}{body}{ParenthesisType.GetClosedChar()}";
    }
    
    public override string GetHashCode()
    {
        var body = $"{LeftExpression.GetHashCode()} {RightExpression.GetHashCode()}";
        if (!HasParenthesis())
            return body;
        return $"({body})";
    }
    
    public override Expression EtaReduction()
    {
        LeftExpression = LeftExpression.EtaReduction();
        RightExpression = RightExpression.EtaReduction();
        return this;
    }
}