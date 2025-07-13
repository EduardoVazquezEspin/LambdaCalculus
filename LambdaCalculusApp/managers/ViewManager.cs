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

    public void Write(ConsoleColor color, params string[] strings)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        foreach (var str in strings)
            Console.WriteLine(str);
        Console.ForegroundColor = originalColor;
        
        AddSom();
    }
    
    public void Write(params string[] strings)
    {
        foreach (var str in strings)
            Console.WriteLine(str);
        
        AddSom();
    }
}