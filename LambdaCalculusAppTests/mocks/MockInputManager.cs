using LambdaCalculusApp.managers.input;

namespace LambdaCalculusAppTests.mocks;

public abstract class MockInput { }

public class MockInputString : MockInput
{
    public string Input { get; set; }
}

public class MockInputChar : MockInput
{
    public char Input { get; set; }
}

public class MockInputKey : MockInput
{
    public ConsoleKeyInfo Input { get; set; } 
}

public class MockInputManager : InputManager
{
    public override Task Run()
    {
        throw new IllegalConsoleUsageDuringTesting();
    }

    public async Task Run(params MockInput[] input)
    {
        List<MockInputKey> inputList = input.SelectMany(it =>
        {
            if (it is MockInputKey itKey)
                return new []{ itKey };
            if (it is MockInputChar itChar)
                return ToConsoleKeyInfo(itChar.Input).Select(id => new MockInputKey {Input = id});
            var itString = (MockInputString) it;
            return itString.Input.SelectMany(c => ToConsoleKeyInfo(c).Select(id => new MockInputKey {Input = id}));
        }).ToList();
        CursorController = new CursorController();
        foreach (var inputObject in inputList)
            await HandleOnType(inputObject.Input);
    }

    private ConsoleKeyInfo[] ToConsoleKeyInfo(char c)
    {
        if(c is >= 'a' and <= 'z')
            return new[] {new ConsoleKeyInfo(c, (ConsoleKey) c.ToString().ToUpper()[0], false, false, false)};
        if(c is >= 'A' and <= 'Z')
            return new[] { new ConsoleKeyInfo(c, (ConsoleKey) c, false, false, false)};
        if(c is >= '0' and <= '9')
            return new[] { new ConsoleKeyInfo(c, (ConsoleKey) c, false, false, false)};
        switch (c)
        {
            case '(':
                return new[] {new ConsoleKeyInfo(c, ConsoleKey.NumPad8, true, false, false)};
            case ')':
                return new[] {new ConsoleKeyInfo(c, ConsoleKey.NumPad9, true, false, false)};
            case '.':
                return new[] {new ConsoleKeyInfo(c, ConsoleKey.OemPeriod, true, false, false)};
            case ' ':
                return new[] {new ConsoleKeyInfo(c, ConsoleKey.Spacebar, true, false, false)};
            case 'λ':
                var lKey = new ConsoleKeyInfo('l', ConsoleKey.L, false, false, false);
                return new[] {lKey, lKey};
            default:
                throw new NotImplementedException();
        }
    }
}