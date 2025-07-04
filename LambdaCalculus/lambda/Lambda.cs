#pragma warning disable CS8618

namespace LambdaCalculus.lambda;

public class Lambda : Expression
{
    private readonly Dictionary<string, Variable> _globalContext;
    private readonly Variable _variable;
    private readonly Expression _expression;

    public Lambda(
        Variable variable,
        Expression expression,
        Dictionary<string, Variable> globalContext
    )
    {
        _variable = variable; 
        _expression = expression;
        _globalContext = globalContext;
    }

    public override string ToString()
    {
        return $"λ{_variable.ToString()}.{_expression.ToString()}";
    }
}