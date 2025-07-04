namespace LambdaCalculus.lambda;

public class Composition : Expression
{
    private readonly List<Expression> _expressions;

    public Composition(List<Expression> expressions)
    {
        _expressions = expressions;
    }

    public override string ToString()
    {
        return string.Join(' ', _expressions.Select(it => it.ToString()));
    }
}