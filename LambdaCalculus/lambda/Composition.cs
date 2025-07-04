namespace LambdaCalculus.lambda;

public class Composition : Expression
{
    private List<Expression> _expressions;

    public Composition(List<Expression> expressions)
    {
        _expressions = expressions;
        _expressions.ForEach(it => it.Parent = this);
    }

    public override string ToString()
    {
        return string.Join(' ', _expressions.Select(it => it.ToString()));
    }

    public override Expression Simplify()
    {
        _expressions = _expressions.Select(it => it.Simplify()).ToList();

        if (_expressions[0] is Parenthesis {Expression: Composition childComposition})
        {
            _expressions.RemoveAt(0);
            var childExpressions = childComposition._expressions;
            childExpressions.ForEach(expression => expression.Parent = this);
            _expressions = childExpressions.Concat(_expressions).ToList();
        }
        
        if (_expressions[^1] is Parenthesis {Expression: Lambda lambda})
        {
            _expressions.RemoveAt(_expressions.Count - 1);
            lambda.Parent = this;
            _expressions.Add(lambda);
        }

        return this;
    }

    public override bool IsWellFormatted()
    {
        return _expressions.TrueForAll(expression => expression is Variable || expression.Parent == this && expression.IsWellFormatted());
    }
}