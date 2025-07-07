namespace LambdaCalculus.lambda;

public class BetaReductionOption
{
    public int Height { get; }
    public int Right { get; }
    public Composition Composition { get; }

    public BetaReductionOption(Composition composition, int height, int right)
    {
        Composition = composition;
        Height = height;
        Right = right;
    }
}