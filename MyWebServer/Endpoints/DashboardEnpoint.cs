using Microsoft.Data.SqlClient;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class DashboardEndpoint : EndpointBase
{
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();
    public virtual ResponseHelper ResponseHelper { get; set; } = new ResponseHelper();
    // [Get("dashboard")]
    // public IHttpResponseResult GetPage()
    // {
    //     var localPath = "index.html";
    //     var responseText = ResponseHelper.GetResponseText(localPath);
    //     return Html(responseText);
    // }
        
    [Get("dashboard")]
    public IHttpResponseResult GetPage()
    {
        var localPath = "Views/Templates/Pages/Dashboard/index.html";
        var responseText = ResponseHelper.GetResponseTextCustomPath(localPath);
        if (!_authorizationHelper.IsAuthorized(Context)) // Используем метод проверки авторизации
        {
            return Redirect("auth/login");
        }

        var templateEngine = new HtmlTemplateEngine();
        var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
        var connectionString = AppConfig.GetInstance().ConnectionString;
        using var sqlConnection = new SqlConnection(connectionString);
        var dbContext = new ORMContext<UserOld>(sqlConnection);
        var user = dbContext.GetById(userId);
        var text = templateEngine.Render(responseText, user);
        return Html(text);
    }
}