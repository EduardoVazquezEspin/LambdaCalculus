namespace LambdaCalculusApp.managers.input;

public abstract class HandlerNode
{
    protected readonly ManagerInjector ManagerInjector;

    protected HandlerNode(ManagerInjector managerInjector)
    {
        ManagerInjector = managerInjector;
    }
}

public abstract class OnSubmitHandlerNode : HandlerNode
{
    protected OnSubmitHandlerNode(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class OnSubmitHandlerNodeSync : OnSubmitHandlerNode
{
    public abstract HandlerResult HandleOnSubmit(string input);
    
    protected OnSubmitHandlerNodeSync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class OnSubmitHandlerNodeAsync : OnSubmitHandlerNode
{
    public abstract Task<HandlerResult> HandleOnSubmit(string input);

    protected OnSubmitHandlerNodeAsync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class OnTypeHandlerNode : HandlerNode
{
    protected OnTypeHandlerNode(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class OnTypeHandlerNodeSync : OnTypeHandlerNode
{
    
    public abstract HandlerResult HandleOnType(ConsoleKeyInfo info);

    protected OnTypeHandlerNodeSync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}

public abstract class OnTypeHandlerNodeAsync : OnTypeHandlerNode
{
    
    public abstract Task<HandlerResult> HandleOnType(ConsoleKeyInfo info);

    protected OnTypeHandlerNodeAsync(ManagerInjector managerInjector) : base(managerInjector)
    {
    }
}