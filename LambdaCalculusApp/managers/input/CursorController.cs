namespace LambdaCalculusApp.managers.input;

public class CursorController
{
    public string Text { get; private set; }
    public int Position { get; private set; }
    public int MinPosition => 1;
    public int MaxPosition => mocked ? 100 : Console.WindowWidth - 3;

    private bool mocked;

    public CursorController()
    {
        mocked = Environment.GetEnvironmentVariable("TESTING") == "TRUE";
        Text = EmptyString(MaxPosition);
        Position = MinPosition;
        UpdateText();
    }

    public void WriteLine(string s)
    {
        if (s.Length >= MaxPosition)
            Text = s[..MaxPosition];
        else
            Text = s + EmptyString(MaxPosition - s.Length);
        UpdateText();
    }
    
    public void Write(params char[] c)
    {
        Write(true, c);
    }
    
    private void Write(bool update, params char[] c)
    {
        var max = Math.Min(c.Length, MaxPosition - Position);
        var originalText = Text;
        Text = Text[..Position];
        for (int i = 0; i < max; i++)
            Text += c[i];
        Text += originalText[(Position+max)..];
        Position += max;
        if(update) UpdateText();
    }

    public void Move(int amount)
    {
        Position = Math.Min(MaxPosition, Math.Max(1, Position + amount));
        UpdateText();
    }

    public char LastChar()
    {
        return Text[Position - 1];
    }

    public void Delete(int amount)
    {
        var safeAmount = Math.Max(0, Math.Min(amount, Position));
        Position -= safeAmount;
        Write(false, EmptyString(safeAmount).ToCharArray());
        Position -= safeAmount;
        UpdateText();
    }

    public void PressEnter()
    {
        Text = EmptyString(MaxPosition);
        Position = MinPosition;
        UpdateText();
    }

    private void UpdateText()
    {
        ClearLine();
        Console.Write(">" + Text);
        Console.CursorLeft = Position + 1;
    }

    private string EmptyString(int length)
    {
        var result = "";
        for (int i = 0; i < length; i++)
            result += " ";
        return result;
    }
    
    private void ClearLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', MaxPosition)); 
        Console.SetCursorPosition(0, currentLineCursor);
    }
}