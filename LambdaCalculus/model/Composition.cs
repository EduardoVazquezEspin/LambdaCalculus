using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

public class Composition : Expression, IParenthesisHolder
{
    internal Expression LeftExpression { get; private set; }

    internal Expression RightExpression { get; private set; }

    public ParenthesisType ParenthesisType { get; set; }

    internal Composition(Expression leftExpression, Expression rightExpression, ParenthesisType parenthesisType = ParenthesisType.Round)
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

    public override Composition Copy()
    {
        var leftExpression = LeftExpression.Copy();
        var rightExpression = RightExpression.Copy();
        return new Composition(leftExpression, rightExpression, ParenthesisType);
    }

    internal override void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath)
    {
        if (LeftExpression is Lambda)
        {
            var path = new List<CompositionPath>(currentPath) {CompositionPath.This};
            list.Add(new BetaReductionOption(path, height, right));
        }
        currentPath.Add(CompositionPath.Left);
        LeftExpression.GetAllBetaReductionOptionsRecursive(list, height + 1, right, currentPath);
        currentPath.RemoveAt(currentPath.Count - 1);
        currentPath.Add(CompositionPath.Right);
        RightExpression.GetAllBetaReductionOptionsRecursive(list, height + 1, right + 1, currentPath);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    public override Expression BetaReduction(BetaReductionOption option)
    {
        var path = option.Path[0];
        option.Path.RemoveAt(0);
        switch (path)
        {
            case CompositionPath.Left:
                LeftExpression = LeftExpression.BetaReduction(option);
                return this;
            case CompositionPath.Right:
                RightExpression = RightExpression.BetaReduction(option);
                return this;
            case CompositionPath.This:
            default:
                break;
        }

        if (LeftExpression is not Lambda lambda)
            throw new Exception("Something went wrong");

        RightExpression.RemoveVariableCalls();
        var result = lambda.Expression.Substitute(lambda.Definition, RightExpression);
        result.Parent = Parent;
        return result;
    }

    internal override Expression Substitute(Definition definition, Expression expression)
    {
        LeftExpression = LeftExpression.Substitute(definition, expression);
        RightExpression = RightExpression.Substitute(definition, expression);
        return this;
    }

    internal override void RemoveVariableCalls()
    {
        LeftExpression.RemoveVariableCalls();
        RightExpression.RemoveVariableCalls();
    }
}