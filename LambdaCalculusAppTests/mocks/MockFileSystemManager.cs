using LambdaCalculusApp;
using LambdaCalculusApp.helpers;

namespace LambdaCalculusAppTests.mocks;

public class MockFileSystemManager : IFileSystemManager
{
    public IResult<string[]>? ReadFileResponse { get; set; }
    public IResult<string[]> ReadFile(string fileName)
    {
        if (ReadFileResponse is null)
            throw new UnMockedApiCall("MockFileSystemManager.ReadFileResponse");
        return ReadFileResponse;
    }
}