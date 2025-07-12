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

    internal virtual int GetContextSize()
    {
        return Parent?.GetContextSize() ?? 0;
    }

    protected virtual Definition? GetLocalVariable(uint id)
    {
        return Parent?.GetLocalVariable(id);
    }
    
    internal virtual Definition? GetLocalVariableByName(string name)
    {
        return Parent?.GetLocalVariableByName(name);
    }

    public abstract Expression EtaReduction();
    
    public abstract Expression Copy();
    public List<BetaReductionOption> GetAllBetaReductionOptions()
    {
        var list = new List<BetaReductionOption>();
        GetAllBetaReductionOptionsRecursive(list, 0, 0, new List<CompositionPath>());
        return list;
    }

    internal abstract void GetAllBetaReductionOptionsRecursive(List<BetaReductionOption> list, int height, int right, List<CompositionPath> currentPath);
    public abstract Expression BetaReduction(BetaReductionOption option);

    internal abstract Expression Substitute(Definition definition, Expression expression);
    internal abstract void RemoveVariableCalls();
    
    public Expression Compute()
    {
        var element = EtaReduction();
        var queue = new PriorityQueue<Expression, int>();

        int minCount = -1;
        Expression minValue = element;

        var elementHash = element.GetHashCode();
        var visited = new HashSet<string> {elementHash};
        queue.Enqueue(element, elementHash.Length);

        while (queue.TryDequeue(out var top, out _) )
        {
            var options = top.GetAllBetaReductionOptions();
            if (options.Count == 0)
                return top;
            if (minCount == -1 || minCount > options.Count)
            {
                minCount = options.Count;
                minValue = top;
            }
            foreach (var option in options)
            {
                var copy = top.Copy();
                var result = copy
                    .BetaReduction(option)
                    .EtaReduction();
                var hash = result.GetHashCode();
                if (!visited.Contains(hash))
                {
                    visited.Add(hash);
                    queue.Enqueue(result, hash.Length);
                }
            }
        }

        return minValue;
    }
}