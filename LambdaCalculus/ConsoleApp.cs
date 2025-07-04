using LambdaCalculus.handlers;
using LambdaCalculus.managers;
using LambdaCalculus.managers.input;

namespace LambdaCalculus;

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
            ViewManager = new ViewManager()
        };

        return this;
    }

    protected virtual ConsoleApp AddHandlers()
    {
        var inputManager = ManagerInjector.InputManager;
        
        inputManager.AddOnSubmitHandler(new EndExecutionHandler(ManagerInjector), new HandlerOptions {Priority = 998});
        inputManager.AddOnSubmitHandler(new RepeatCryptic(ManagerInjector), new HandlerOptions {Priority = 999});
        inputManager.AddOnSubmitHandler(new InvalidCrypticHandler(ManagerInjector), new HandlerOptions {Priority = 1000});

        return this;
    }
    
    public virtual async Task Run()
    {
        ManagerInjector.ViewManager.WelcomeView();
        await ManagerInjector.InputManager.Run();
    }
}