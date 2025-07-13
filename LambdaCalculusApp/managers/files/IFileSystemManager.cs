using LambdaCalculusApp.helpers;

namespace LambdaCalculusApp;

public interface IFileSystemManager
{
    public IResult<string[]> ReadFile(string fileName);
}