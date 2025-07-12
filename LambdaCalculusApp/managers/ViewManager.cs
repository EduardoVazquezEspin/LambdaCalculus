namespace LambdaCalculusApp.managers;

public class ViewManager
{
    private void AddSom()
    {
        Console.Write("> ");
    }

    public void WelcomeView()
    {
        Console.WriteLine("Welcome to the lambda runner environment");
        AddSom();
    }
    
    public void InvalidCrypticView()
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Input!");
        Console.ForegroundColor = originalColor;
        AddSom();
    }
    
    public void EndSessionView()
    {
        Console.WriteLine("Bye!");
    }
}