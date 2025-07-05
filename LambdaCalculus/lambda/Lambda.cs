namespace LambdaCalculus.lambda;

public class Lambda : Expression
{
    private readonly Dictionary<string, Variable> _globalContext;
    private readonly Variable _variable;
    public Variable Variable => _variable;
    private Expression _expression;
    public Expression Expression => _expression;

    public Lambda(
        Variable variable,
        Expression expression,
        Dictionary<string, Variable> globalContext
    )
    {
        _variable = variable;
        _variable.Parent = this;
        _expression = expression;
        _expression.Parent = this;
        _globalContext = globalContext;
    }

    public override Expression Simplify()
    {
        _expression = _expression.Simplify();

        // Lambda of Parenthesis is lambda
        if (_expression is Parenthesis innerParenthesis)
            _expression = innerParenthesis.Expression;
        
        _expression.Parent = this;
        
        return this;
    }

    public override bool IsWellFormatted()
    {
        return _expression is Variable || _expression.Parent == this && _expression.IsWellFormatted();
    }

    public override int GetContextSize()
    {
        return base.GetContextSize() + 1;
    }

    public override string ToString()
    {
        return $"λ{_variable.ToString()}.{_expression.ToString()}";
    }
    
    public override string GetHashCode()
    {
        return $"λ{_expression.GetHashCode()}";
    }
}