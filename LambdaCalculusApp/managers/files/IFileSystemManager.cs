using LambdaCalculusApp.common;

namespace LambdaCalculusApp;

public interface IFileSystemManager
{
    public IResult<string[]> ReadFile(string fileName);
}