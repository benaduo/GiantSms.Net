namespace GiantSms.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private Exception? _exceptionToThrow;
    private HttpResponseMessage? _response;

    public void SetupResponse(HttpResponseMessage response)
    {
        _response = response;
        _exceptionToThrow = null!; 
    }

    public void SetupException(Exception exception)
    {
        _exceptionToThrow = exception;
        _response = null!; 
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_exceptionToThrow != null) throw _exceptionToThrow;

        return Task.FromResult(_response!);
    }
}