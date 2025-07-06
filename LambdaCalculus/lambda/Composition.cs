namespace LambdaCalculus.lambda;

public class Composition : Expression
{
    internal List<Expression> Expressions { get; private set; }

    public Composition(List<Expression> expressions)
    {
        Expressions = expressions;
        Expressions.ForEach(it => it.Parent = this);
    }
    public override Expression Simplify()
    {
        Expressions = Expressions.Select(it => it.Simplify()).ToList();

        // Composition is left associative
        if (Expressions[0] is Parenthesis {Expression: Composition childComposition})
        {
            Expressions.RemoveAt(0);
            var childExpressions = childComposition.Expressions;
            childExpressions.ForEach(expression => expression.Parent = this);
            Expressions = childExpressions.Concat(Expressions).ToList();
        }
        // Lambdas extend to the right
        if (Expressions[^1] is Parenthesis {Expression: Lambda lambda})
        {
            Expressions.RemoveAt(Expressions.Count - 1);
            lambda.Parent = this;
            Expressions.Add(lambda);
        }

        return this;
    }

    internal override Expression EtaReductionRecursive()
    {
        Expressions = Expressions.Select(expression => expression.EtaReductionRecursive()).ToList();
        return this;
    }

    public override bool IsWellFormatted()
    {
        return Expressions.TrueForAll(expression => expression is Variable || expression.Parent == this && expression.IsWellFormatted());
    }

    public override string ToString()
    {
        return string.Join(' ', Expressions.Select(it => it.ToString()));
    }
    
    public override string GetHashCode()
    {
        return string.Join(' ', Expressions.Select(it => it.GetHashCode()));
    }
}