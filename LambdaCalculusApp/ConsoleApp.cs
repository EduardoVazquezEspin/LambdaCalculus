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
            LambdaManager = new LambdaManager(),
            FileSystemManager = new FileSystemManager()
        };

        return this;
    }

    protected virtual ConsoleApp AddHandlers()
    {
        var inputManager = ManagerInjector.InputManager;

        inputManager.AddOnTypeHandlers(
            new OnTypeSpecialCharactersHandler(ManagerInjector),
            new OnTypeEnterHandler(ManagerInjector),
            new OnTypeLHandler(ManagerInjector),
            new OnTypeBiggerThanHandler(ManagerInjector),
            new OnTypeArrowHandler(ManagerInjector),
            new OnTypeDefaultHandler(ManagerInjector)
        );
        
        inputManager.AddOnSubmitHandler(new AddToHistoryHandler(ManagerInjector), new HandlerOptions {Priority = -1});
        inputManager.AddOnSubmitHandlers(
            new DefineGlobalHandler(ManagerInjector),
            new LoadFileHandler(ManagerInjector)
        );
        inputManager.AddOnSubmitHandler(new ComputeLambdaHandler(ManagerInjector), new HandlerOptions {Priority = 500});
        inputManager.AddOnSubmitHandler(new EndExecutionHandler(ManagerInjector), new HandlerOptions {Priority = 499});
        inputManager.AddOnSubmitHandler(new InvalidCrypticHandler(ManagerInjector), new HandlerOptions {Priority = 1000});

        return this;
    }
    
    public virtual async Task Run()
    {
        ManagerInjector.ViewManager.WelcomeView();
        await ManagerInjector.InputManager.Run();
    }
}