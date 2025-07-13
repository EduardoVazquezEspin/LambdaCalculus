using LambdaCalculus;

namespace LambdaCalculusApp.managers;

public class LambdaManager
{
    public LambdaParser Parser { get; }
    
    public List<Expression> ResultHistory { get; private set; }
    public List<string> InputHistory { get; set; }

    public LambdaManager()
    {
        Parser = new LambdaParser();
        ResultHistory = new List<Expression>();
        InputHistory = new List<string>();
    }
}