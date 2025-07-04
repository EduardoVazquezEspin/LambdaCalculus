namespace LambdaCalculus.lambda;

internal enum Flow
{
    Continue, // Character has been accepted
    ParseExpression, // Character has not been accepted. Start parsing again and return the value to me. Type could be any
    ParseNewVariable,
    ParseVariable,
    ParseLambda,
    ParseParenthesis,
    Build, // Character has not been accepted. Build me and return me
    Error  // Character is invalid
}

internal static class Constants
{
    public static readonly char[] ValidOpenParenthesis =  {'(', '[', '{'};
    public static readonly char[] ValidClosedParenthesis =  {')', ']', '}'};
}

internal static class Helpers
{
    public static bool IsValidOpenParenthesis(char c)
    {
        return Constants.ValidOpenParenthesis.Contains(c);
    }

    public static bool IsValidClosedParenthesis(char c)
    {
        return Constants.ValidClosedParenthesis.Contains(c);
    }
    
    public static bool IsValidVariableChar(char c)
    {
        if (c is >= 'a' and <= 'z')
            return true;
        if (c is >= 'A' and <= 'Z')
            return true;
        return false;
    }
}
