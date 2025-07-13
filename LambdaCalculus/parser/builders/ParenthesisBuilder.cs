namespace LambdaCalculus;

internal class ParenthesisBuilder : AbstractExpressionBuilder
{
    private enum ParenthesisBuilderState
    {
        ReadingOpen,
        ReadingExpression,
        ReadingClosed,
        Finished
    }

    private ParenthesisBuilderState _state;
    private ParenthesisType _type;
    private Expression? _expression;
    
    
    public ParenthesisBuilder(AbstractExpressionBuilder? parent = null) : base(parent)
    {
        _state = ParenthesisBuilderState.ReadingOpen;
    }

    public override Flow Analyze(char c)
    {
        if (c == ' ') return Flow.Continue;
        return _state switch
        {
            ParenthesisBuilderState.ReadingOpen => AnalyzeReadingOpen(c),
            ParenthesisBuilderState.ReadingExpression => AnalyzeReadingExpression(c),
            ParenthesisBuilderState.ReadingClosed => AnalyzeReadingClosed(c),
            ParenthesisBuilderState.Finished => AnalyzeFinished(c),
            _ => Flow.Error
        };
    }

    private Flow AnalyzeReadingOpen(char c)
    {
        if (!Helpers.IsValidOpenParenthesis(c))
            return Flow.Error;
        _type = c.GetParenthesisType();
        _state = ParenthesisBuilderState.ReadingExpression;
        return Flow.Continue;
    }

    private Flow AnalyzeReadingExpression(char c)
    {
        return Flow.ParseExpression;
    }

    private Flow AnalyzeReadingClosed(char c)
    {
        if (!Helpers.IsValidClosedParenthesis(c))
            return Flow.Error;
        var newType = c.GetParenthesisType();
        if (newType != _type)
            return Flow.Error;
        _state = ParenthesisBuilderState.Finished;
        return Flow.Continue;
    }

    private Flow AnalyzeFinished(char c)
    {
        return Flow.Build;
    }

    public override void BackToYou(Expression lastParsedExpression)
    {
        if (_state != ParenthesisBuilderState.ReadingExpression)
            return;
        _expression = lastParsedExpression;
        _state = ParenthesisBuilderState.ReadingClosed;
    }

    public override Expression? Build(out ParseError? error)
    {
        error = null;
        if (_expression == null)
        {
            error = new UnfinishedExpression();
            return null;
        }

        if (_expression is IParenthesisHolder parenthesisHolder)
            parenthesisHolder.ParenthesisType = _type;
        return _expression;
    }
}