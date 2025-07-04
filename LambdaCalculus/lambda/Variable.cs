namespace LambdaCalculus.lambda;

public class Variable : Expression
{
    private readonly string _name;
    public string Name => _name;

    public Variable(string name)
    {
        _name = name;
    }

    public override string ToString()
    {
        return _name;
    }
}