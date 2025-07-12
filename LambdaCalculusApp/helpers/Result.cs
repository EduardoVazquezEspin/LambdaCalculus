namespace LambdaCalculusApp.helpers;

public interface IResult
{
    public bool Success { get; set; }
    public List<string> Messages { get; set; }
}

public class Result : IResult
{
    public bool Success { get; set; }
    public List<string> Messages { get; set; } = new();
}

public interface IResult<T> : IResult
{
    public T Value { get; set; }
}

public class Result<T> : IResult<T>
{
    public bool Success { get; set; }
    public List<string> Messages { get; set; } = new();
    public T Value { get; set; }
}