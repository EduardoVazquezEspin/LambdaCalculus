namespace LambdaCalculus;

internal class NewVariableBuilder : AbstractExpressionBuilder
{
    private enum NewVariableBuilderState{
        ReadingWhitespace,
        ReadingText
    }

    private NewVariableBuilderState _state;
    private string _text;

    public NewVariableBuilder(AbstractExpressionBuilder? parent = null) : base(parent)
    {
        _state = NewVariableBuilderState.ReadingWhitespace;
        _text = "";
    }

    public override Flow Analyze(char c)
    {
        switch (_state)
        {
            case NewVariableBuilderState.ReadingWhitespace:
                return AnalyzeReadingWhiteSpace(c);
            case NewVariableBuilderState.ReadingText:
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
            _state = NewVariableBuilderState.ReadingText;
            _text += c;
            return Flow.Continue;
        }
        return Flow.Error;
    }

    private Flow AnalyzeReadingText(char c)
    {
        if (c == ' ' || c == '.')
            return Flow.Build;
        if (Helpers.IsValidVariableChar(c))
        {
            _text += c;
            return Flow.Continue;
        }
        return Flow.Error;
    }

    public override Expression Build(out ParseError? error)
    {
        error = null;
        var variable = new Definition(_text, 0);
        return variable;
    }
}