namespace LambdaCalculusApp.managers.input;

public class HandlerOptions
{
    public int Priority { get; init; }
}

public class InputManager
{
    private readonly  List<KeyValuePair<List<OnSubmitHandlerNode>, int>>  _onSubmitHandlers;
    private readonly List<OnTypeHandlerNode> _onTypeHandlers;
    public CursorController CursorController { get; private set; }

    public InputManager()
    {
        _onSubmitHandlers = new List<KeyValuePair<List<OnSubmitHandlerNode>, int>>();
        _onTypeHandlers = new List<OnTypeHandlerNode>();
    }

    public Action AddOnSubmitHandler(OnSubmitHandlerNode handlerNode, HandlerOptions? options = null)
    {
        options ??= new HandlerOptions();
        var priority = options.Priority;
        var samePriority = _onSubmitHandlers.Where(it => it.Value == priority).ToList();
        var list = samePriority.Count != 0 ? samePriority[0].Key : null;
        if (list is null)
        {
            list = new List<OnSubmitHandlerNode>();
            _onSubmitHandlers.Add(new KeyValuePair<List<OnSubmitHandlerNode>, int>(list, priority));
            _onSubmitHandlers.Sort((a,b) => a.Value -b.Value);
        }
        list.Add(handlerNode);

        return () => { list.Remove(handlerNode); };
    }

    public Action[] AddOnSubmitHandlers(params OnSubmitHandlerNode[] handlerNodes)
    {
        return handlerNodes.Select(it => AddOnSubmitHandler(it)).ToArray();
    }

    public Action AddOnTypeHandler(OnTypeHandlerNode handlerNode)
    {
        _onTypeHandlers.Add(handlerNode);
        return () => { _onTypeHandlers.Add(handlerNode); };
    }

    public Action[] AddOnTypeHandlers(params OnTypeHandlerNode[] handlerNodes)
    {
        return handlerNodes.Select(AddOnTypeHandler).ToArray();
    }

    public async Task<HandlerResult> HandleOnSubmit(string input)
    {
        Flow flow = Flow.Continue;
        HandlerResult? finalResult = null;
        for (int i = 0; flow == Flow.Continue && i < _onSubmitHandlers.Count; i++)
        {
            var list = _onSubmitHandlers[i].Key;
            for (int j = 0; flow == Flow.Continue && j < list.Count; j++)
            {
                var handlerNode = list[j];
                if (handlerNode is OnSubmitHandlerNodeSync handlerNodeSync)
                    finalResult = handlerNodeSync.HandleOnSubmit(input);
                else
                    finalResult = await ((OnSubmitHandlerNodeAsync) handlerNode).HandleOnSubmit(input);
                flow = finalResult.Flow;
            }
        }
        return finalResult ?? new HandlerResult(false, new List<string>(), Flow.Continue);
    }

    public async Task<HandlerResult> HandleOnType(ConsoleKeyInfo c)
    {
        for (int i = 0; i < _onTypeHandlers.Count; i++)
        {
            var handler = _onTypeHandlers[i];
            HandlerResult result;
            if (handler is OnTypeHandlerNodeSync syncHandler)
                result = syncHandler.HandleOnType(c);
            else
                result = await ((OnTypeHandlerNodeAsync) handler).HandleOnType(c);
            if (result.Flow != Flow.Continue)
                return result;
        }
        return new HandlerResult(false, new List<string>(), Flow.Continue);
    }

    public async Task Run()
    {
        CursorController = new CursorController();
        bool end = false;
        while (!end)
        {
            var c = Console.ReadKey();
            var result = await HandleOnType(c);
            end = result.Flow == Flow.EndExecution;
        }
    }
}