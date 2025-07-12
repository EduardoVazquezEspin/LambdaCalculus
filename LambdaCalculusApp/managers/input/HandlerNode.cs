namespace LambdaCalculusApp.managers.input;

public abstract class HandlerNode
{
    protected readonly ManagerInjector ManagerInjector;

    protected HandlerNode(ManagerInjector managerInjector)
    {
        ManagerInjector = managerInjector;
    }
}

public abstract class HandlerNodeSync : HandlerNode
{
    public abstract HandlerResult HandleOnSubmit(string input);
    
    protected HandlerNodeSync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class HandlerNodeAsync : HandlerNode
{
    public abstract Task<HandlerResult> HandleOnSubmit(string input);

    protected HandlerNodeAsync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}