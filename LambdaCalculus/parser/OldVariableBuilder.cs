namespace LambdaCalculus;

internal class OldVariableBuilder : AbstractExpressionBuilder
{
    private enum OldVariableBuilderState{
        ReadingWhitespace,
        ReadingText
    }

    private OldVariableBuilderState _state;
    private string _text;

    public OldVariableBuilder(AbstractExpressionBuilder? parent = null) : base(parent)
    {
        _state = OldVariableBuilderState.ReadingWhitespace;
        _text = "";
    }

    public override Flow Analyze(char c)
    {
        switch (_state)
        {
            case OldVariableBuilderState.ReadingWhitespace:
                return AnalyzeReadingWhiteSpace(c);
            case OldVariableBuilderState.ReadingText:
                return AnalyzeReadingText(c);
            default:
                return Flow.Error;
        }
    }

    private Flow AnalyzeReadingWhiteSpace(char c)
    {
        if (c == ' ')
            return Flow.Continue;
        if (Helpers.IsValidVariableChar(c))
        {
            _state = OldVariableBuilderState.ReadingText;
            _text += c;
            return Flow.Continue;
        }
        return Flow.Error;
    }

    private Flow AnalyzeReadingText(char c)
    {
        if (c == ' ' || c == '.' || c == '\n' || Helpers.IsValidClosedParenthesis(c) || Helpers.IsValidOpenParenthesis(c))
            return Flow.Build;
        if (Helpers.IsValidVariableChar(c))
        {
            _text += c;
            return Flow.Continue;
        }

        return Flow.Error;
    }
    

    public override void BackToYou(Expression lastParsedExpression) { }

    public override Expression? Build(out ParseError? error)
    {
        error = null;
        var definition = GetLocalVariable(_text);
        if (definition is null)
        {
            error = new FreeVariableBuilder{Length = _text.Length};
            return null;
        }

        definition.Calls++;
        return new Variable(definition);
    }
}