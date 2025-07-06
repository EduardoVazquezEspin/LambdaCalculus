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

    internal override Expression EtaReductionRecursive()
    {
        _expression = _expression.EtaReductionRecursive();
        
        if (
            _variable.Calls != 1 || 
            _expression is not Composition composition ||
            composition.Expressions[^1] != _variable
        ) return this;

        if (composition.Expressions.Count == 2)
        {
            var result = composition.Expressions[0];
            if (Parent is not Composition)
            {
                result.Parent = Parent;
                return result;
            }
            var newParenthesis = new Parenthesis(ParenthesisType.Round, result);
            result.Parent = newParenthesis;
            newParenthesis.Parent = Parent;
            return newParenthesis;
        }
        
        composition.Expressions.RemoveAt(composition.Expressions.Count - 1);
        if (Parent is not Composition)
        {
            composition.Parent = Parent;
            return composition;
        }
        var parenthesis = new Parenthesis(ParenthesisType.Round, composition);
        composition.Parent = parenthesis;
        parenthesis.Parent = Parent;
        return parenthesis;
    }

    public override bool IsWellFormatted()
    {
        return _expression is Variable || _expression.Parent == this && _expression.IsWellFormatted();
    }

    protected override int GetContextSize()
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