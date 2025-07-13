namespace LambdaCalculusApp.managers;

public class ViewManager
{
    public void WelcomeView()
    {
        Console.WriteLine("Welcome to the lambda runner environment");
    }
    
    public void InvalidCrypticView()
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Input!");
        Console.ForegroundColor = originalColor;
    }
    
    public void EndSessionView()
    {
        Console.WriteLine("Bye!");
    }

    public void Write(ConsoleColor color, params string[] strings)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        foreach (var str in strings)
            Console.WriteLine(str);
        Console.ForegroundColor = originalColor;
    }
    
    public void Write(params string[] strings)
    {
        foreach (var str in strings)
            Console.WriteLine(str);
    }
}