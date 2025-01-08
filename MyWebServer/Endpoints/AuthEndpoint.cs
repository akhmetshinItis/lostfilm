using Microsoft.Data.SqlClient;
using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Core;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Models;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class AuthEndpoint : EndpointBase
{
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();

    [Get("auth/login")]
    public IHttpResponseResult Get()
    {
        var localPath = "Views/Templates/Pages/Auth/signin.html";
        var responseText = File.ReadAllText(localPath);
        return Html(responseText);
    }
    
    [Post("auth/login")]
    public IHttpResponseResult Login(string login, string password)
    {
        var connectionString = AppConfig.GetInstance().ConnectionString;
        using var sqlConnection = new SqlConnection(connectionString);
        var dbContext = new ORMContext<UserOld>(sqlConnection);
        var user = dbContext.FirstOrDefault(x => x.Login == login && x.Password == password);
        if (user == null)
        {
            var localPath = "Views/Templates/Pages/Auth/signin.html";
            var responseText = File.ReadAllText(localPath);
            return Html(responseText);
        }
        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("session-token", token);
        nameCookie.Path = "/";
        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, user.Id.ToString());
        return Redirect("/dashboard");
    }

    [Get("auth/register")]
    public IHttpResponseResult Register()
    {
        var localPath = "Views/Templates/Pages/Auth/signup.html";
        var responseText = File.ReadAllText(localPath);
        return Html(responseText);
    }

    [Post("auth/register")]
    public IHttpResponseResult Register(string login, string password, string firstName, string lastName)
    {
        var connectionString = AppConfig.GetInstance().ConnectionString;
        using var sqlConnection = new SqlConnection(connectionString);
        var dbContext = new ORMContext<UserOld>(sqlConnection);
        var check = dbContext.FirstOrDefault(x => x.Login == login);
        
        if (check is not null)
        {
            return Redirect("/auth/login");
        }
        
        var user = new UserOld { Login = login, Password = password, Name = $@"{firstName} {lastName}"};
        dbContext.Create(user);

        user.Id = dbContext.FirstOrDefault(u => u.Login == login).Id; 
        
        var token = Guid.NewGuid().ToString();
        Cookie nameCookie = new Cookie("session-token", token);
        nameCookie.Path = "/";
        Context.Response.Cookies.Add(nameCookie);
        SessionStorage.SaveSession(token, user.Id.ToString());
        return Redirect("/dashboard");
    }
    
}
// с кошельком заходить только когда залогинен