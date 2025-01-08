namespace HttpServerLibrary.Core.HttpResponse;

public class RedirectResult: IHttpResponseResult
{
    private readonly string _location;
    public RedirectResult(string location)
    {
        _location = location;
    }
 
    public void Execute(HttpRequestContext context)
    {
        var response = context.Response;
        response.StatusCode = 302;
        response.Headers.Add("Location", _location); // Заголовок для указания пути
        response.Close();
    }
}