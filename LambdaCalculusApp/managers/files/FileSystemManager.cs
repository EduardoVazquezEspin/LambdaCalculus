using System.Text.RegularExpressions;
using LambdaCalculusApp.common;

namespace LambdaCalculusApp;

public class FileSystemManager : IFileSystemManager
{
    private readonly Regex _fileNameRegex = new Regex(@"\.?[A-Z]+(\.[A-Z]+)*", RegexOptions.IgnoreCase);
    
    public IResult<string[]> ReadFile(string fileName)
    {
        var match = _fileNameRegex.Match(fileName);
        if (!match.Success)
            return new Result<string[]>
            {
                Success = false,
                Messages = new List<string> {"Invalid filename"}
            };
        
        IResult<string[]> result;
        
        try
        {
            var fileContent = File.ReadAllLines(fileName);
            result = new Result<string[]>
            {
                Success = true,
                Value = fileContent
            };
        }
        catch (Exception e)
        {
            result = new Result<string[]>
            {
                Success = false,
                Messages = new List<string> {e.Message}
            };
        }

        return result;
    }
}