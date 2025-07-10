namespace LambdaCalculus.lambda;

public abstract class Expression
{
    public Expression? Parent { get; internal set; }

    public sealed override string ToString()
    {
        var cache = new Dictionary<uint, string>();
        var taken = new HashSet<string>();
        return ToString(cache, taken);
    }
    internal abstract string ToString(Dictionary<uint, string> cache, HashSet<string> taken);
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

    protected virtual Variable? GetLocalVariable(uint id)
    {
        return Parent?.GetLocalVariable(id);
    }

    internal virtual Variable? GetLocalVariableByName(string name)
    {
        return Parent?.GetLocalVariableByName(name);
    }

    public abstract Expression Copy();

    protected Expression CopyChild(Expression expression)
    {
        if (expression is not Variable variable)
            return expression.Copy();
        
        var result = GetLocalVariable(variable.Id);
        if (result is null)
            throw new Exception("Something went wrong");
        result.Calls++;
        return result;
    }

    public abstract Expression EtaReduction();

    public List<BetaReductionOption> GetAllBetaReductionOptions()
    {
        var list = new List<BetaReductionOption>();
        GetAllBetaReductionOptionsRecursive(list, 0, 0, new List<CompositionPath>());
        return list;
    }

    internal abstract void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath);
    public abstract Expression BetaReduction(BetaReductionOption option);

    public Expression Compute()
    {
        var element = EtaReduction();
        var queue = new List<KeyValuePair<Expression, BetaReductionOption>> { };
        var options0 = element.GetAllBetaReductionOptions();
        if (options0.Count == 0)
            return element;

        int minCount = options0.Count;
        Expression minValue = element;
        
        foreach (var option in options0)
            queue.Add(new KeyValuePair<Expression, BetaReductionOption>(element, option));
        var visited = new HashSet<string> {GetHashCode()};

        while (queue.Any())
        {
            var top = queue[0];
            queue.RemoveAt(0);
            var copy = top.Key.Copy();
            var result = copy
                .BetaReduction(top.Value)
                .EtaReduction();
            var hash = result.GetHashCode();
            if (!visited.Contains(hash))
            {
                visited.Add(hash);
                var options = result.GetAllBetaReductionOptions();
                if (options.Count == 0)
                    return result;
                if (minCount > options.Count)
                {
                    minCount = options.Count;
                    minValue = result;
                }
                foreach (var option in options)
                    queue.Add(new KeyValuePair<Expression, BetaReductionOption>(result, option));
            }
        }

        return minValue;
    }

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