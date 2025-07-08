namespace LambdaCalculus.lambda;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }
    public new abstract string ToString();
    public new abstract string GetHashCode();

    public new bool Equals(object? obj)
    {
        return obj is Expression objExpression && Equals(objExpression.GetHashCode(), GetHashCode());
    }

    public virtual bool IsWellFormatted()
    {
        return true;
    }

    protected virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }

    protected virtual Variable? GetLocalVariable(string name)
    {
        return Parent?.GetLocalVariable(name);
    }

    public abstract Expression Copy();

    protected Expression CopyChild(Expression expression)
    {
        if (expression is not Variable variable)
            return expression.Copy();
        
        var result = GetLocalVariable(variable.Name);
        if (result is null)
            throw new Exception("Something went wrong");
        result.Calls++;
        return result;
    }

    public abstract Expression EtaReduction();

    public List<BetaReductionOption> GetAllBetaReductionOptions()
    {
        var list = new List<BetaReductionOption>();
        GetAllBetaReductionOptionsRecursive(list, 0, 0);
        return list;
    }

    internal abstract void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right);
    public abstract Expression BetaReduction(BetaReductionOption option);

    internal abstract void RemoveVariableCalls();

    protected abstract Expression Substitute(Variable variable, Expression expression);

    protected Expression SubstituteChild(Expression child, Variable variable, Expression expression)
    {
        if (child is not Variable childVariable)
            return child.Substitute(variable, expression);

        if (variable != childVariable)
            return child;
        
        return expression.Copy();
    }
}