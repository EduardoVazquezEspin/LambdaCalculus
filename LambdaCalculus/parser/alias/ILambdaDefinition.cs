using System.Diagnostics.CodeAnalysis;

namespace LambdaCalculus;

public interface ILambdaDefinition
{
    public bool TryGetExpression(string name, [NotNullWhen(true)] out Expression? expression);

    public bool TryGetAlias(Expression expression, [NotNullWhen(true)] out string alias);
}