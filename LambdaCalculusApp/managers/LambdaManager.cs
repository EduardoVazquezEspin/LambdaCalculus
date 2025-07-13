using LambdaCalculus;

namespace LambdaCalculusApp.managers;

public class LambdaManager
{
    public LambdaParser Parser { get; }
    
    public List<Expression> History { get; private set; }

    public LambdaManager()
    {
        Parser = new LambdaParser();
        History = new List<Expression>();
    }
}