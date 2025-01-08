using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Models;
using my_http.Models.Tests;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class IVITestEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();

    [Get("ivi")]
    public IHttpResponseResult Index()
    {
        var localPath = "Views/Templates/Pages/Tests/IVI/index.html";
        var templateEngine = new HtmlTemplateEngine();
        var text = _responseHelper.GetResponseTextCustomPath(localPath);
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<UserOld>(connection);
        var users = dbContext.GetAll();
        var model = new TestPageVm { Users = users };
        var page = templateEngine.Render(text, model);
        return Html(page);
    }
}