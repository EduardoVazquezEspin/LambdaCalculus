namespace LambdaCalculus;

public enum CompositionPath
{
    Left,
    Right,
    This
}

public class BetaReductionOption
{
    public int Height { get; }
    public int Right { get; }
    public List<CompositionPath> Path { get; }

    internal BetaReductionOption(List<CompositionPath> path, int height, int right)
    {
        Path = path;
        Height = height;
        Right = right;
    }
}
