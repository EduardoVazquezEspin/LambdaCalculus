namespace LambdaCalculusAppTests.mocks;

public class UnMockedApiCall : Exception
{
    public UnMockedApiCall(string mocker) : base($"This API call should be mocked by using {mocker}") {}
}

public class IllegalConsoleUsageDuringTesting : Exception
{
    public IllegalConsoleUsageDuringTesting() : base($"You can't use a console app during testing!!") {}
}