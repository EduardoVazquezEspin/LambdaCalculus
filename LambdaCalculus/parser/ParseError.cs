namespace LambdaCalculus;

public abstract class ParseError
{
    public abstract string Message { get; }
}

public class NoError : ParseError
{
    public override string Message => "OK";
}

public class SomethingWentWrong : ParseError
{
    public override string Message => "Something went wrong while parsing the expression";
}

public class InvalidCharacter : ParseError
{
    public override string Message { get; }

    public InvalidCharacter(string expressionStr, int index)
    {
        var first = expressionStr[..index];
        var character = expressionStr[index];
        var second = expressionStr.Substring(index + 1, expressionStr.Length - index - 1);
        Message = $"Invalid character found at position {index}:\n{first}>{character}<{second}";
    }
}

public class UnfinishedExpression : ParseError
{
    public override string Message => "Unfinished expression";
}

public class EmptyExpression : ParseError
{
    public override string Message => "Empty expression";
}

internal class FreeVariableBuilder : ParseError
{
    public override string Message => "";
    
    public int Length { get; set; }
}

public class FreeVariable : ParseError
{
    public override string Message { get; }
    public FreeVariable(string expressionStr, int indexStart, int indexEnd)
    {
        var first = expressionStr[..indexStart];
        var variable = expressionStr.Substring(indexStart, indexEnd - indexStart);
        var second = expressionStr.Substring(indexEnd, expressionStr.Length - indexEnd);
        Message = $"Invalid free variable:\n{first}>{variable}<{second}";
    }
}