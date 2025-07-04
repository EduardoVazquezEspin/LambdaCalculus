namespace LambdaCalculus.lambda;

public class Variable : Expression
{
    public string Name { get; }

    public Variable(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }

    public override Expression Simplify()
    {
        return this;
    }
}