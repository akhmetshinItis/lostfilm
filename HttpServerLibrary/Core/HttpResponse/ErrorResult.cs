using System.Text;

namespace HttpServerLibrary.Core.HttpResponse;

public class ErrorResult : IHttpResponseResult
{
    
    private readonly string _error;

    
    public string GetError()
    {
        return _error;
    }
    
    
    public ErrorResult(string error)
    {
        _error = error;
    }

    
    public void Execute(HttpRequestContext context)
    {
        var text = $@"
<!DOCTYPE html>
<html lang='ru'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Ошибка</title>
        <style>
            body {{
                font-family: Arial, sans-serif;
                text-align: center;
                margin: 0;
                padding: 20px;
                background-color: #f8f9fa;
                color: #343a40;
            }}
            h1 {{
                color: #dc3545;
            }}
            a {{
                text-decoration: none;
                color: #007bff;
                font-weight: bold;
            }}
            a:hover {{
                text-decoration: underline;
            }}
        </style>
    </head>
    <body>
        <h1>Произошла ошибка</h1>
        <p>{System.Net.WebUtility.HtmlEncode(_error)}</p>
        <a href='/index'>На главную</a>
    </body>
</html>";
        byte[] buffer = Encoding.UTF8.GetBytes(text);

        context.Response.ContentLength64 = buffer.Length;
        using Stream output = context.Response.OutputStream;

        output.Write(buffer);
        output.Flush();
    }
}