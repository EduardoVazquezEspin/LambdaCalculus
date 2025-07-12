namespace LambdaCalculus;

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

internal interface IParenthesisHolder
{
    public ParenthesisType ParenthesisType { get; set; }
}