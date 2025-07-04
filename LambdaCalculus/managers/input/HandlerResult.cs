namespace LambdaCalculus.managers.input;

public enum Flow
{
    Continue,
    StopPropagation,
    EndExecution
}

public class HandlerResult
{
    public bool Success { get; set; }
    public List<string> Messages { get; set; }
    public Flow Flow { get; set; }

    public HandlerResult(Flow flow = Flow.StopPropagation)
    {
        Success = false;
        Messages = new List<string>();
        Flow = flow;
    }

    public HandlerResult(bool success, List<string> messages, Flow flow = Flow.StopPropagation)
    {
        Success = success;
        Messages = messages;
        Flow = flow;
    }
}