namespace LambdaCalculus.lambda;

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