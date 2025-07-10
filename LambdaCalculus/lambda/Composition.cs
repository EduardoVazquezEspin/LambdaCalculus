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
            throw new Exception("Not well formatted");
        if (RightExpression is not Variable && (RightExpression.Parent != this || !RightExpression.IsWellFormatted()))
            throw new Exception("Not well formatted");
        return true;
    }

    public override Composition Copy()
    {
        var originalLeftExpressionParent = LeftExpression.Parent;
        var leftExpression = CopyChild(LeftExpression);
        var originalRightExpressionParent = RightExpression.Parent;
        var rightExpression = CopyChild(RightExpression);
        var newComposition = new Composition(leftExpression, rightExpression, ParenthesisType);
        if (LeftExpression is Variable)
            LeftExpression.Parent = originalLeftExpressionParent;
        if (RightExpression is Variable)
            RightExpression.Parent = originalRightExpressionParent;
        return newComposition;
    }

    public override Expression EtaReduction()
    {
        LeftExpression = LeftExpression.EtaReduction();
        RightExpression = RightExpression.EtaReduction();
        return this;
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath)
    {
        if (LeftExpression is Lambda)
        {
            var copy = new List<CompositionPath>(currentPath) {CompositionPath.This};
            list.Add(new BetaReductionOption(copy, height, right));
        }
            
        currentPath.Add(CompositionPath.Left);
        LeftExpression.GetAllBetaReductionOptionsRecursive(list, height + 1, right, currentPath);
        currentPath.RemoveAt(currentPath.Count - 1);
        currentPath.Add(CompositionPath.Right);
        RightExpression.GetAllBetaReductionOptionsRecursive(list, height, right + 1, currentPath);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    public override Expression BetaReduction(BetaReductionOption option)
    {
        var direction = option.Path[0];
        option.Path.RemoveAt(0);
        switch (direction)
        {
            case CompositionPath.Left:
                LeftExpression = LeftExpression.BetaReduction(option);
                return this;
            case CompositionPath.Right:
                RightExpression = RightExpression.BetaReduction(option);
                return this;
            case CompositionPath.This:
            default:
                RightExpression.RemoveVariableCalls();
                if (LeftExpression is not Lambda lambda)
                    throw new Exception("Something went wrong");
                var substitution = SubstituteChild(lambda.Expression, lambda.Variable, RightExpression);
                if(substitution is not Variable)
                    substitution.Parent = Parent;
                return substitution;
        }
    }

    internal override void RemoveVariableCalls()
    {
        LeftExpression.RemoveVariableCalls();
        RightExpression.RemoveVariableCalls();
    }

    protected override Expression Substitute(Variable variable, Expression expression)
    {
        LeftExpression = SubstituteChild(LeftExpression, variable, expression);
        if(LeftExpression is not Variable)
            LeftExpression.Parent = this;
        RightExpression = SubstituteChild(RightExpression, variable, expression);
        if(RightExpression is not Variable)
            RightExpression.Parent = this;
        return this;
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
}