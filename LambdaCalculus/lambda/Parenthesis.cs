namespace LambdaCalculus.lambda;

public enum ParenthesisType
{
    Round,
    Square,
    Curly
}

public static class ParenthesisTypeHelpers {
    public static char GetOpenChar(this ParenthesisType type) => 
        type switch
        {
            ParenthesisType.Round => '(',
            ParenthesisType.Square => '[',
            ParenthesisType.Curly => '{',
            _ => throw new Exception("Invalid type")
        };
    
    public static char GetClosedChar(this ParenthesisType type) => 
        type switch
        {
            ParenthesisType.Round => ')',
            ParenthesisType.Square => ']',
            ParenthesisType.Curly => '}',
            _ => throw new Exception("Invalid type")
        };
    
    public static ParenthesisType GetParenthesisType(this char c) =>
        c switch
        {
            '(' or ')' => ParenthesisType.Round,
            '[' or ']' => ParenthesisType.Square,
            '{' or '}' => ParenthesisType.Curly,
            _ => throw new Exception("Invalid char")
        };
}

public class Parenthesis : Expression
{
    private readonly ParenthesisType _type;
    public ParenthesisType Type => _type;
    private Expression _expression;
    public Expression Expression => _expression;

    public Parenthesis(
        ParenthesisType type,
        Expression expression
        )
    {
        _type = type;
        _expression = expression;
        expression.Parent = this;
    }
    
    public override string ToString()
    {
        return _type.GetOpenChar() + _expression.ToString() + _type.GetClosedChar();
    }

    public override Expression Simplify()
    {
        // Global parenthesis of not composition
        if (Parent == null)
        {
            _expression.Parent = null;
            var result = _expression.Simplify();
            return result;
        }

        _expression = _expression.Simplify();

        if (_expression is Parenthesis innerParenthesis)
        {
            _expression = innerParenthesis.Expression;
            _expression.Parent = this;
        }
            

        if (_expression is Variable)
        {
            _expression.Parent = Parent;
            return _expression;
        }
        
        return this;
    }
    
    public override bool IsWellFormatted()
    {
        var validity =_expression is Variable ||  _expression.Parent == this && _expression.IsWellFormatted();
        return validity;
    }
}