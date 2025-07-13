namespace LambdaCalculusApp.managers.input;

public class HandlerOptions
{
    public int Priority { get; init; }
}

public class InputManager
{
    private readonly  List<List<HandlerNode>>  _onSubmitHandlers;
    private readonly  List<int> _onSubmitPriorities;

    public InputManager()
    {
        _onSubmitHandlers = new List<List<HandlerNode>>();
        _onSubmitPriorities = new List<int>();
    }

    public Action AddOnSubmitHandler(HandlerNode handlerNode, HandlerOptions? options = null)
    {
        options ??= new HandlerOptions();
        var priority = options.Priority;
        var index = _onSubmitPriorities.IndexOf(priority);
        List<HandlerNode> list;
        if (index == -1)
        {
            list = new List<HandlerNode>();
            _onSubmitPriorities.Add(priority);
            _onSubmitHandlers.Add(list);

            var isSorted = false;
            for (int i = _onSubmitPriorities.Count - 2; !isSorted && i >= 0; i--)
            {
                isSorted = _onSubmitPriorities[i] < priority;
                if (!isSorted)
                {
                    (_onSubmitPriorities[i], _onSubmitPriorities[i + 1]) = (_onSubmitPriorities[i + 1], _onSubmitPriorities[i]);
                    (_onSubmitHandlers[i], _onSubmitHandlers[i + 1]) = (_onSubmitHandlers[i + 1], _onSubmitHandlers[i]);
                }
            }
        }
        else
        {
            list = _onSubmitHandlers[index];
        }
        list.Add(handlerNode);

        return () => { list.Remove(handlerNode); };
    }

    public Action[] AddOnSubmitHandlers(params HandlerNode[] handlerNodes)
    {
        return handlerNodes.Select(it => AddOnSubmitHandler(it)).ToArray();
    }

    public async Task<HandlerResult> HandleOnSubmit(string input)
    {
        Flow flow = Flow.Continue;
        HandlerResult finalResult = new HandlerResult(false, new List<string>(), Flow.Continue);
        for (int i = 0; flow == Flow.Continue && i < _onSubmitPriorities.Count; i++)
        {
            var list = _onSubmitHandlers[i];
            for (int j = 0; flow == Flow.Continue && j < list.Count; j++)
            {
                var handlerNode = list[j];
                if (handlerNode is HandlerNodeSync handlerNodeSync)
                    finalResult = handlerNodeSync.HandleOnSubmit(input);
                else
                    finalResult = await ((HandlerNodeAsync) handlerNode).HandleOnSubmit(input);
                flow = finalResult.Flow;
            }
        }
        return finalResult;
    }

    public async Task Run()
    {
        bool end = false;
        while (!end)
        {
            var line = Console.ReadLine() ?? "";
            var result = await HandleOnSubmit(line);
            end = result.Flow == Flow.EndExecution;
        }
    }
}