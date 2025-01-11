using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Models.Entities;
using MyHttp.Repositories;
using TemlateEngine;

namespace my_http.Endpoints;

public class AuthEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    
    [Get("login")]
    public IHttpResponseResult Index()
    {
        var localPath = "Sign-In.html";
        var responseText = _responseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    [Post("login")]
    public IHttpResponseResult Login(string login, string password)
    {
        try
        {
            var repository = new Repository<User>();
            var user = repository.FirstOrDefault(u => u.Login == login);
            if (user is null || user.Password != password)
            {
                return Redirect("login");
            }

            var token = Guid.NewGuid().ToString();
            Cookie nameCookie = new Cookie("session-token", token);
            nameCookie.Path = "/";
            Context.Response.Cookies.Add(nameCookie);
            SessionStorage.SaveSession(token, user.Id.ToString());

            return Redirect("index");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }
}
