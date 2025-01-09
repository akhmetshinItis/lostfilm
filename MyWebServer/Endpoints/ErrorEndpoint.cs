using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Models.ErrorPage;
using TemlateEngine;

namespace my_http.Endpoints;

public class ErrorEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();

    [Get("error")]
    public IHttpResponseResult GetErrorPage(string error)
    {
        var localPath = "Views/Templates/Pages/ErrorPage/Error.html";
        var page = _responseHelper.GetResponseTextCustomPath(localPath);
        var model = new ErrorDataVm { Error = error };
        page = _htmlTemplateEngine.Render(page, model);
        return Html(page);
    }
}