using LambdaCalculusApp.handlers;
using LambdaCalculusApp.managers;
using LambdaCalculusApp.managers.input;

namespace LambdaCalculusApp;

public class ConsoleApp
{
    public ManagerInjector ManagerInjector = null!;

    public ConsoleApp()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        ConfigureManagers();

        // ReSharper disable once VirtualMemberCallInConstructor
        AddHandlers();
    }

    protected virtual ConsoleApp ConfigureManagers()
    {
        ManagerInjector = new ManagerInjector
        {
            InputManager = new InputManager(),
            ViewManager = new ViewManager(),
            LambdaManager = new LambdaManager()
        };

        return this;
    }

    protected virtual ConsoleApp AddHandlers()
    {
        var inputManager = ManagerInjector.InputManager;

        inputManager.AddOnSubmitHandlers(
            new DefineGlobalHandler(ManagerInjector)
        );
        inputManager.AddOnSubmitHandler(new ComputeLambdaHandler(ManagerInjector), new HandlerOptions {Priority = 500});
        inputManager.AddOnSubmitHandler(new EndExecutionHandler(ManagerInjector), new HandlerOptions {Priority = 998});
        inputManager.AddOnSubmitHandler(new InvalidCrypticHandler(ManagerInjector), new HandlerOptions {Priority = 1000});

        return this;
    }
    
    public virtual async Task Run()
    {
        ManagerInjector.ViewManager.WelcomeView();
        await ManagerInjector.InputManager.Run();
    }
}